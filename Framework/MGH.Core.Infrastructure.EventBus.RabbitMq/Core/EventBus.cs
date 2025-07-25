﻿using System.Reflection;
using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connection;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Core;

public class EventBus: IEventBus
{
    private readonly IRabbitConnection _rabbitConnection;
    private readonly IServiceProvider _serviceProvider;
    public EventBus(IRabbitConnection rabbitConnection, IServiceProvider serviceProvider)
    {
        _rabbitConnection = rabbitConnection;
        _serviceProvider = serviceProvider;
        _rabbitConnection.ConnectService();
    }
    
    public void Publish<T>(T model) where T : IEvent
    {
        _rabbitConnection.ConnectService();

        var basicProperties = _rabbitConnection.GetChannel().CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        PrepareToPublish(baseMessage);
        
        _rabbitConnection.GetChannel().BasicPublish(
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
        
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();
        
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
    public void Consume<T>(Func<T, Task> handler) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();
        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        PrepareToPublish(baseMessage);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json);
                if (message != null)
                    await handler(message);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(baseMessage.QueueName, false, consumer);
    }

    public void Consume<T>() where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();
        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        PrepareToPublish(baseMessage);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                var handler = scope.ServiceProvider.GetService<IEventHandler<T>>();
                if (handler == null)
                    throw new InvalidOperationException($"Handler for event type {typeof(T).Name} not registered.");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (message == null)
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); // discard message
                    return;
                }

                await handler.HandleAsync(message);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"❌ Error handling message for type {typeof(T).Name}: {ex.Message}");
                Console.WriteLine($"Raw message: {json}");

                // Optional: log to Serilog or ILogger here
                channel.BasicNack(ea.DeliveryTag, false, false); // reject and don't requeue
            }
        };

        channel.BasicConsume(queue: baseMessage.QueueName, autoAck: false, consumer: consumer);
    }

  
    private void PrepareToPublish(BaseMessage baseMessage)
    {
        _rabbitConnection.GetChannel().ExchangeDeclare(
            exchange: baseMessage.ExchangeName,
            type: baseMessage.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        _rabbitConnection.GetChannel().QueueDeclare(baseMessage.QueueName, true, false, false, null);
        _rabbitConnection.GetChannel().QueueBind(baseMessage.QueueName, baseMessage.ExchangeName, baseMessage.RoutingKey);
    }
    private BaseMessage GetBaseMessageFromAttribute(Type type)
    {
        var attribute = type.GetCustomAttributes(typeof(EventAttribute), true)
            .FirstOrDefault() as EventAttribute;

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
