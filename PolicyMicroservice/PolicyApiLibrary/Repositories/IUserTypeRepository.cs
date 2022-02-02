using PolicyApiLibrary.DbModels;
using System.Collections.Generic;

namespace PolicyApiLibrary.Repositories
{
    public interface IUserTypeRepository
    {
        IEnumerable<UserType> GetUserTypes();

        UserType GetUserTypeByName(string userTypeName);
    }
}
