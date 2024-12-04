using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;
using MGH.Core.Infrastructure.MessageBroker.RabbitMq.Atributes;

namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public RabbitMqPublisher(IRabbitMqConnection connection)
    {
        _rabbitMqConnection = connection;
        _rabbitMqConnection.ConnectService();
    }

    public void Publish<T>(T model)
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