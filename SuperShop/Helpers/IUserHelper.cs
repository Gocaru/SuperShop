using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        //Método que recebu uma string com o email e indica que user é
        Task<User> GetUserByEmailAsync(string email);

        //Método para criar o user
        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
