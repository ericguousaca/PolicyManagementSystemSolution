using CommonLibrary.Messaging.Commands;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PolicyManagementWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Repositories
{
    /// <summary>
    /// Add or get Search Policy Result from MonogoDB
    /// </summary>
    public class MongoSearchPolicyRepository : ISearchPolicyRepository
    {
        private readonly IMongoCollection<ISearchPolicyResultCommand> _searchPolicyResults;
        public MongoSearchPolicyRepository(IMongoPolicyDbSettings settings)
        {
            BsonClassMap.RegisterClassMap<SearchPolicyResultCommand>();
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.MongoPolicyDbName);
            this._searchPolicyResults = database.GetCollection<ISearchPolicyResultCommand>(settings.SearchPolicyCollectionName);
        }

        /// <summary>
        /// Get search policy result command object from MongoDB
        /// </summary>
        /// <param name="searchId">The unique search Id</param>
        /// <returns>A search policy result command object <see cref="ISearchPolicyResultCommand"></see></returns>
        public async Task<ISearchPolicyResultCommand> GetSearchPolicyResultCommand(string searchId)
        {
            List<ISearchPolicyResultCommand> resultCommands = await this._searchPolicyResults.Find(x => true).ToListAsync();
            ISearchPolicyResultCommand resultCommand = resultCommands.Where(x => x.SearchPolicyCommand.SearchId == searchId).FirstOrDefault();

            return resultCommand;
        }

        /// <summary>
        /// Add search result command object into MongoDB
        /// </summary>
        /// <param name="resultCommand">An search result command object<see cref="SearchPolicyResultCommand"/></param>
        /// <returns></returns>
        public async Task AddSearchPolicyResultCommand(SearchPolicyResultCommand resultCommand)
        {
            await this._searchPolicyResults.InsertOneAsync(resultCommand);
        }
    }
}
