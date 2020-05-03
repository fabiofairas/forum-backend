using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Project.Model.Settings;
using System.Threading.Tasks;

namespace Project.Data
{
    public class Context
    {
        private readonly IOptions<CosmosDBSettings> _settings;

        private readonly CosmosClient _cosmosClient;

        private string _databaseId = string.Empty;
        private string _containerId = string.Empty;
        public string partitionKey = string.Empty;

        private Database database;
        public Container container;
        public Context(IOptions<CosmosDBSettings> settings)
        {
            _cosmosClient = new CosmosClient(settings.Value.EndpointUri, settings.Value.PrimaryKey, new CosmosClientOptions() { ApplicationName = settings.Value.ApplicationName });

            _databaseId = settings.Value.DatabaseId;
            _containerId = settings.Value.ContainerId;

            partitionKey = settings.Value.PartitionKey;

            _settings = settings;
        }

        public async Task initializeAsync()
        {
            await CreateDatabaseAsync();
            await CreateContainerAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            if (_settings.Value.database == null)
            {
                database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
                _settings.Value.database = database;
            }
            else
            {
                database = _settings.Value.database;
            }
        }

        private async Task CreateContainerAsync()
        {
            if (_settings.Value.container == null)
            {
                container = await database.CreateContainerIfNotExistsAsync(_containerId, "/PartitionName", 400);
                _settings.Value.container = container;
            }
            else
            {
                container = _settings.Value.container;
            }
        }
    }
}
