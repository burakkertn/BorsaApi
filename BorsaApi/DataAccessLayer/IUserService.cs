using BorsaApi.Concrete;
using System.Collections.Generic;

namespace BorsaApi.DataAccessLayer
{
    public interface IUserService
    {
        List<User> Get();
        User Get(string id);
      
        User Create(User user);
        void Remove(string id);
    }
}