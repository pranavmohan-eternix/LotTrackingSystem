using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetByUsername(string username);
    }
}