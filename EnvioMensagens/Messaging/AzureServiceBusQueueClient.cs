using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public class AzureServiceBusQueueClient : IClientMessageBroker
    {
        private IConfiguration _configuration;
        private Logger _logger;
        private string _destination;

        public void Initialize(IConfiguration configuration, Logger logger, string destination)
        {
            _configuration = configuration;
            _logger = logger;
            _destination = destination;
        }

        public void SendMessages(string[] messages)
        {
            var client = new QueueClient(
                _configuration.GetConnectionString("AzureServiceBus"),
                _destination,
                ReceiveMode.ReceiveAndDelete);

            foreach (var message in messages)
            {
                var body = Encoding.UTF8.GetBytes(message);
                client.SendAsync(new Message(body)).Wait();

                _logger.Information($"Azure Service Bus - " +
                    $"Queue: {_destination} - Mensagem enviada: {message}");
            }
        }
    }
}