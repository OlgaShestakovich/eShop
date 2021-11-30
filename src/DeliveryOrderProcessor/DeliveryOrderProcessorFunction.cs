using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeliveryOrderProcessor
{
    public static class DeliveryOrderProcessorFunction
    {
        [FunctionName("DeliveryOrderProcessorFunction")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            ResponceModel newOrder = JsonConvert.DeserializeObject<ResponceModel>(requestBody);

            var cosmosService = new CosmosService();

            _ = cosmosService.AddNewOrderToContainerAsync(newOrder, newOrder.PartitionKey);

            string responseMessage = newOrder == null
                ? "This HTTP triggered function executed successfully. But data is false."
                : $"{newOrder}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
