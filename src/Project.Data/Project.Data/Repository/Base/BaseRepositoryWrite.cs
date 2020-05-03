using Microsoft.Extensions.Options;
using Project.Model.Settings;
using System.Threading.Tasks;

namespace Project.Data.Repository.Base
{
    public class BaseRepositoryWrite
    {
        public readonly Context _context;
        public BaseRepositoryWrite(IOptions<CosmosDBSettings> settings)
        {
            _context = new Context(settings);
        }

        public async Task InitializeAsync()
        {
            await _context.initializeAsync();
        }
    }
}