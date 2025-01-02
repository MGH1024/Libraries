using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Abstracts;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Attributes;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Concrete;

public class EventBusDispatcher: IEventBusDispatcher
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public EventBusDispatcher(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _rabbitMqConnection.ConnectService();
    }
    
    public void Publish<T>(T model) where T : IEvent
    {
        _rabbitMqConnection.ConnectService();

        var basicProperties = _rabbitMqConnection.GetChannel().CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        PrepareToPublish(baseMessage);
        
        _rabbitMqConnection.GetChannel().BasicPublish(
            exchange: baseMessage.ExchangeName,
            routingKey: baseMessage.RoutingKey,
            basicProperties: basicProperties,
            body: messageByte
        );
    }

    public void Publish<T>(IEnumerable<T> models) where T : IEvent
    {
        if (models == null || !models.Any())
            throw new ArgumentException("The collection of models cannot be null or empty.", nameof(models));
        
        _rabbitMqConnection.ConnectService();
        var channel = _rabbitMqConnection.GetChannel();
        
        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        PrepareToPublish(baseMessage);
        
        var basicProperties = channel.CreateBasicProperties();
        foreach (var model in models)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageByte = Encoding.UTF8.GetBytes(messageJson);
            
            channel.BasicPublish(
                exchange: baseMessage.ExchangeName,
                routingKey: baseMessage.RoutingKey,
                basicProperties: basicProperties,
                body: messageByte
            );
        }
    }

    private void PrepareToPublish(BaseMessage baseMessage)
    {
        _rabbitMqConnection.GetChannel().ExchangeDeclare(
            exchange: baseMessage.ExchangeName,
            type: baseMessage.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        _rabbitMqConnection.GetChannel().QueueDeclare(baseMessage.QueueName, true, false, false, null);
        _rabbitMqConnection.GetChannel().QueueBind(baseMessage.QueueName, baseMessage.ExchangeName, baseMessage.RoutingKey);
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
}
