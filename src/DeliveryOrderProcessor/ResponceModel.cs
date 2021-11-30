using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeliveryOrderProcessor
{
    public class ResponceModel
    {
        public ResponceModel(string id, string partitionKey, string addressShipToAddress, IReadOnlyCollection<object> orderItems)
        {
            Id = id;
            PartitionKey = partitionKey;
            AddressShipToAddress = addressShipToAddress;
            OrderItems = orderItems;

        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "buyerId")]
        public string PartitionKey { get; private set; }

        public string AddressShipToAddress { get; private set; }

        public IReadOnlyCollection<object> OrderItems { get; private set; }
    }
}
