using Microsoft.Azure.Cosmos;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryOrderProcessor
{
    public class CosmosService
    {
        public async Task<HttpStatusCode> AddNewOrderToContainerAsync<T>(T newOrder, string partitionKey)
        {
            CosmosClient _cosmosClient = new CosmosClient("AccountEndpoint=https://cosmosdb-e-shop.documents.azure.com:443/;AccountKey=rExr75Rx1CUT9JXdq5FtgP9tFTrql7rkvbDq75MTijyo0L7FPvcw38R8nIocREcDlyIXNJyq8fHJEaIcwT6rDw==;");

            Database _cosmosDb = _cosmosClient.GetDatabase("Orders");


            Container _container = _cosmosDb.GetContainer("NewOrders");

            try
            {
                await _container.CreateItemAsync<T>(newOrder, new PartitionKey(partitionKey));
                return HttpStatusCode.OK;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                return HttpStatusCode.Conflict;
            }
        }
    }
}
