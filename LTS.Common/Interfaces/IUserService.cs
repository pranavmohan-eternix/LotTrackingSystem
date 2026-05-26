using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface IUserService
    {
        void Register(User user);
        User Login(string username, string password);
    }
}