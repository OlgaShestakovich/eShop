using System;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBusFunction
{
    public static class SendOrderToBob
    {
        [FunctionName("SendOrderToBob")]
        [FixedDelayRetry(3, "00:00:02")]
        public static async System.Threading.Tasks.Task<ActionResult> RunAsync([ServiceBusTrigger("queueorders", Connection = "ConnectionStringsCosmos")] string message, ILogger log)
        {
            
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {message}");
            
            ServiceBusMessageModel model = JsonConvert.DeserializeObject<ServiceBusMessageModel>(message);

            var _blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storaccaunttest123;AccountKey=ig52owWS5ys9oKT3TrShdfFZ7dZTTeM68QN54VOHVK1FHzATEDU7IZMvCoxwgwWZY3dg4WERMV0BSTBvp0Fbqg==;EndpointSuffix=core.windows.net");
            var containerName = "new-orders";
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            var fileName = string.Format($"new_order_{DateTime.Now:dd.mm.yy.hh.mm.ss}.json");

            var blobClient = containerClient.GetBlobClient(fileName);

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                await blobClient.UploadAsync(memoryStream);
            }


            return model == null
                ? new BadRequestObjectResult("Please pass order parameters in the request body")
                : (ActionResult)new OkObjectResult(model);

        }
    }
}
