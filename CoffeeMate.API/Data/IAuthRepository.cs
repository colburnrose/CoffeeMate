using System.Threading.Tasks;
using CoffeeMate.API.Models;

namespace CoffeeMate.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string pwd);
         Task<User> Login(string username, string pwd);
         Task<bool> UserExists(string username);
    }
}