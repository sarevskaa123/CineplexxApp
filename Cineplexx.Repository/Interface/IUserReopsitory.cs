
using Cineplexx.Domain.Identity;
using System.Collections.Generic;

namespace Cineplexx.Repository.Interface
{
    public interface IUserReopsitory
    {
        IEnumerable<CineplexxUser> GetAll();
        CineplexxUser Get(string id);
        void Insert(CineplexxUser user);
        void Update(CineplexxUser user);
        void Delete(CineplexxUser user);

    }
}
