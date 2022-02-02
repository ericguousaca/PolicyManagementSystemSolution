using CommonLibrary.Messaging.Commands;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Repositories
{
    /// <summary>
    /// Add or get Search Policy from Database 
    /// </summary>
    public interface ISearchPolicyRepository
    {
        /// <summary>
        /// Add search result command object into DB
        /// </summary>
        /// <param name="resultCommand">An search result command object<see cref="SearchPolicyResultCommand"/></param>
        /// <returns></returns>
        Task AddSearchPolicyResultCommand(SearchPolicyResultCommand resultCommand);

        /// <summary>
        /// Get search policy result command object from DB
        /// </summary>
        /// <param name="searchId">The unique search Id</param>
        /// <returns>A search policy result command object <see cref="ISearchPolicyResultCommand"></see></returns>
        Task<ISearchPolicyResultCommand> GetSearchPolicyResultCommand(string searchId);
    }
}
