﻿namespace MGH.Core.Infrastructure.MessageBroker.RabbitMq.Model;

public class BaseMessage(string routingKey, string exchangeName, string exchangeType, string queueName)
{
    public string RoutingKey { get; set; } = routingKey;
    public string ExchangeName { get; set; } = exchangeName;
    public string ExchangeType { get; set; } = exchangeType;
    public string QueueName { get; set; } = queueName;
}