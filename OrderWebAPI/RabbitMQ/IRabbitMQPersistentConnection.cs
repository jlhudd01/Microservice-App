using System;
using RabbitMQ.Client;

namespace OrderWebAPI.RabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();         
    }
}