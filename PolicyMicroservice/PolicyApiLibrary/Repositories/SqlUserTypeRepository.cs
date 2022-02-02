using PolicyApiLibrary.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace PolicyApiLibrary.Repositories
{
    public class SqlUserTypeRepository : IUserTypeRepository
    {
        public readonly PolicyDbContext _policyDbContext;

        public SqlUserTypeRepository(PolicyDbContext policyDbContext)
        {
            this._policyDbContext = policyDbContext;
        }

        public UserType GetUserTypeByName(string userTypeName)
        {
            return this._policyDbContext.UserTypes
                            .Where(x => x.TypeName.ToUpper() == userTypeName.Trim().ToUpper())
                            .FirstOrDefault();
        }

        public IEnumerable<UserType> GetUserTypes()
        {
            return this._policyDbContext.UserTypes;
        }
    }
}
