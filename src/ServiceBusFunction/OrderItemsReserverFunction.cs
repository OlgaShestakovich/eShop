using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusFunction
{
    public class OrderItemsReserverFunction
    {
        [FunctionName("OrderItemsReserverFunction")]
        public async Task RunAsync([ServiceBusTrigger("queueorders", Connection = "ConnectionStringsCosmos")] 
        string message, ILogger log)
        {
            
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {message}");

            var _blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storaccaunttest123;AccountKey=ig52owWS5ys9oKT3TrShdfFZ7dZTTeM68QN54VOHVK1FHzATEDU7IZMvCoxwgwWZY3dg4WERMV0BSTBvp0Fbqg==;EndpointSuffix=core.windows.net");
            var containerName = "new-orders";
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            var fileName = string.Format($"new_order.json");

            var blobClient = containerClient.GetBlobClient(fileName);

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                await blobClient.UploadAsync(memoryStream);
            }
        
        }
    }
}
