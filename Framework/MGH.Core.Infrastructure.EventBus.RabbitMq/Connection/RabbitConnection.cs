using MGH.Core.Infrastructure.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Connection;

public class RabbitConnection : IRabbitConnection
{
    private Policy _connectionPolicy;
    private ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private bool _isDisposed;
    private bool IsServiceConnected => _connection is not null && _connection.IsOpen;
    private bool IsChannelConnected => _channel is not null && _channel.IsOpen;
    
    public RabbitConnection(IOptions<EventBusConfig> options)
    {
        CreateConnectionPolicy();
        CreateConnectionFactory(options.Value);
        ConnectService();
    }

    public IModel GetChannel()
    {
        return _channel;
    }
    
    public void Dispose()
    {
        if (_isDisposed) return;
        _channel?.Dispose();
        _connection?.Dispose();

        _isDisposed = true;
    }
    public void ConnectService()
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
    
    private void CreateConnectionPolicy()
    {
        _connectionPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 1000,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5)
            );
    }
    
    private void CreateConnectionFactory(EventBusConfig eventBusConfig)
    {
        _connectionFactory = new ConnectionFactory
        {
            UserName = eventBusConfig.DefaultConnection.Username,
            Password = eventBusConfig.DefaultConnection.Password,
            VirtualHost = eventBusConfig.DefaultConnection.VirtualHost,
            HostName = eventBusConfig.DefaultConnection.Host,
            Port = Convert.ToInt32(eventBusConfig.DefaultConnection.Port),
        };
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