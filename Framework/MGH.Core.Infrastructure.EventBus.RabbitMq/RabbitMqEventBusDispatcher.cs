using Polly;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using MGH.Core.Domain.BaseEntity.Abstract;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Atributes;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public class RabbitMqEventBusDispatcher : IEventBusDispatcher 
{
    private Policy _connectionPolicy;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private bool _isDisposed;
    private bool IsServiceConnected => _connection is not null && _connection.IsOpen;
    private bool IsChannelConnected => _channel is not null && _channel.IsOpen;

    public RabbitMqEventBusDispatcher(IOptions<Model.RabbitMq> options)
    {
        CreateConnectionPolicy();
        CreateConnectionFactory(options.Value);
        ConnectService();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _channel?.Dispose();
        _connection?.Dispose();

        _isDisposed = true;
    }

    public void PublishAsync<T>(T item)  where T : IntegratedEvent
    {
        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        PublishWithBaseMessage<T>(item,baseMessage);
    }
    
    private void PublishWithBaseMessage<T>(T model,BaseMessage baseMessage)
    {
        ConnectService();

        var basicProperties = _channel.CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        PrepareToPublish(baseMessage);
        _channel.BasicPublish(
            exchange: baseMessage.ExchangeName,
            routingKey: baseMessage.RoutingKey,
            basicProperties: basicProperties,
            body: messageByte
        );
    }
    
    private BaseMessage GetBaseMessageFromAttribute(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(BaseMessageAttribute), true)
            .FirstOrDefault() as BaseMessageAttribute;

        if (attribute == null)
        {
            throw new InvalidOperationException($"BaseMessageAttribute is not defined for type {type.Name}.");
        }

        return new BaseMessage(
            routingKey: attribute.RoutingKey,
            exchangeType: attribute.ExchangeType,
            exchangeName: attribute.ExchangeName,
            queueName: attribute.QueueName
        );
    }
    
    private void PrepareToPublish(BaseMessage baseMessage)
    {
        _channel.ExchangeDeclare(
            exchange: baseMessage.ExchangeName,
            type: baseMessage.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        _channel.QueueDeclare(baseMessage.QueueName, true, false, false, null);
        _channel.QueueBind(baseMessage.QueueName, baseMessage.ExchangeName, baseMessage.RoutingKey);
    }

    private void CreateConnectionPolicy()
    {
        _connectionPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 1000,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5)
            );
    }

    private void CreateConnectionFactory(Model.RabbitMq rabbitMq)
    {
        _connectionFactory = new ConnectionFactory
        {
            UserName = rabbitMq.DefaultConnection.Username,
            Password = rabbitMq.DefaultConnection.Password,
            VirtualHost = rabbitMq.DefaultConnection.VirtualHost,
            HostName = rabbitMq.DefaultConnection.Host,
            Port = Convert.ToInt32(rabbitMq.DefaultConnection.Port),
        };
    }

    private void ConnectService()
    {
        _connectionPolicy.Execute(() =>
        {
            if (!_isDisposed && !IsServiceConnected)
            {
                _connection?.Dispose();
                _connection = _connectionFactory.CreateConnection(clientProvidedName: "Publisher Connection");
                _connection.CallbackException += Connection_CallbackException;
                _connection.ConnectionBlocked += Connection_ConnectionBlocked;
                _connection.ConnectionShutdown += Connection_ConnectionShutdown;
            }

            ConnectChannel();
        });
    }

    private void ConnectChannel()
    {
        if (_isDisposed || IsChannelConnected) return;
        _channel?.Dispose();

        _channel = _connection.CreateModel();
        _channel.CallbackException += Channel_CallbackException;
    }

    private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e) => ConnectService();

    private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e) => ConnectService();

    private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e) => ConnectService();

    private void Channel_CallbackException(object sender, CallbackExceptionEventArgs e) => ConnectService();
}
