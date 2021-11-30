using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusFunction
{
    public class SeviceBusService
    {

        private readonly string _connectionString;
        private readonly string _queueName;
        static IQueueClient queueClient;

        public SeviceBusService()
        {
            _connectionString = "Endpoint=sb://servicebus-orders-for-reservation.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pNRbB/kZNWrQj690lbPfreYHhYIwrTvE9VQv4SrEF8g=";
            _queueName = "queueorders";
        }
        public async Task SendMessageAsync(string message)
        {
            queueClient = new QueueClient(_connectionString, _queueName);
            var data = JsonConvert.SerializeObject(message);
            var messageCosmos = new Message(Encoding.UTF8.GetBytes(data));
            await queueClient.SendAsync(messageCosmos);
        }
    }
}
