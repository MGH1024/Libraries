using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public class RabbitMqService<T> : IMessageSender<T>
{
    private Policy _connectionPolicy;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private bool _isDisposed;
    private bool IsServiceConnected => _connection is not null && _connection.IsOpen;
    private bool IsChannelConnected => _channel is not null && _channel.IsOpen;


    public RabbitMqService(IOptions<Model.RabbitMq> options)
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

    public void Publish(MessageModel<T> model)
    {
        ConnectService();

        var basicProperties = _channel.CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model.Item);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        PrepareToPublish(model);
        _channel.BasicPublish(exchange: model.BaseMessage.ExchangeName, routingKey: model.BaseMessage.RoutingKey,
            basicProperties: basicProperties, body: messageByte);
    }

    public void Publish(BatchMessageModel<T> model)
    {
        ConnectService();

        var basicProperties = _channel.CreateBasicProperties();
        var basicPublishBatch = _channel.CreateBasicPublishBatch();

        model.Items
            .Select(message => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)).AsMemory())
            .ToList()
            .ForEach(messageByte => basicPublishBatch.Add(model.BaseMessage.ExchangeName, model.BaseMessage.RoutingKey,
                true, basicProperties, messageByte));

        PrepareToPublish(model);
        basicPublishBatch.Publish();
    }

    private void PrepareToPublish(BatchMessageModel<T> model)
    {
        _channel.ExchangeDeclare(exchange: model.BaseMessage.ExchangeName, type: model.BaseMessage.ExchangeType, durable: true,
            autoDelete: false, arguments: null);
        _channel.QueueDeclare(model.BaseMessage.QueueName, true, false, false, null);
        _channel.QueueBind(model.BaseMessage.QueueName, model.BaseMessage.ExchangeName, model.BaseMessage.RoutingKey);
    }

    private void PrepareToPublish(MessageModel<T> model)
    {
        _channel.ExchangeDeclare(exchange: model.BaseMessage.ExchangeName, type: model.BaseMessage.ExchangeType, durable: true,
            autoDelete: false, arguments: null);
        _channel.QueueDeclare(model.BaseMessage.QueueName, true, false, false, null);
        _channel.QueueBind(model.BaseMessage.QueueName, model.BaseMessage.ExchangeName, model.BaseMessage.RoutingKey);
    }

    private void CreateConnectionPolicy()
    {
        _connectionPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 1000,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5),
                onRetry: (exception, time) =>
                {
                    // Console.WriteLine(exception.Message);
                    // Console.WriteLine($"Retry after {time}");
                });
    }

    private void CreateConnectionFactory(Model.RabbitMq rabbitMq)
    {
        _connectionFactory = new ConnectionFactory()
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
        if (!_isDisposed && !IsChannelConnected)
        {
            _channel?.Dispose();

            _channel = _connection.CreateModel();
            _channel.CallbackException += Channel_CallbackException;

            //Console.WriteLine("Channel connected.");
        }
    }

    private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        ConnectService();
    }

    private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        ConnectService();
    }

    private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
    {
        ConnectService();
    }

    private void Channel_CallbackException(object sender, CallbackExceptionEventArgs e)
    {
        ConnectService();
    }
}