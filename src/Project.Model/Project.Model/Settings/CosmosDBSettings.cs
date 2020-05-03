using Microsoft.Azure.Cosmos;

namespace Project.Model.Settings
{
    public class CosmosDBSettings
    {
        public string EndpointUri;
        public string PrimaryKey;
        public string DatabaseId;
        public string ContainerId;
        public string PartitionKey;
        public string ApplicationName;

        public Database database;
        public Container container;
    }
}