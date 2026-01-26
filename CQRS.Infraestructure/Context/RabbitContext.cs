using CQRS.Infraestructure.Context.MongoConfig;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CQRS.Infraestructure.Context
{
    public class RabbitContext : IAbstractFactory<ConnectionFactory>
    {
        private readonly ConnectionFactory _connection;

        public RabbitContext(IOptions<RabbitConfig> options)
        {
            _connection = new ConnectionFactory { HostName = options.Value.Connection };
        }

        public ConnectionFactory CreateConnection()
        {
            return _connection;
        }
    }
}
