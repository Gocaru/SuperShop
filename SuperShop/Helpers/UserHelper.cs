using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Construtor da classe "UserHelper" que recebe uma instância de "UserManager{User}
        /// </summary>
        /// <param name="userManager">Instância do "UserManager{User}" injetada pelo sistema de dependência,
        /// configurada para gerir operações relacionadas com a entidade "User"</param>
        public UserHelper(UserManager<User> userManager)  //Injeto o "UserManager" com o meu user personalizado
        {
            _userManager = userManager;
            //Armazeno a instância injetada de UserManager<User> no campo privado da classe,
            //para que possa ser usada nos métodos da classe UserHelper, como AddUserAsync ou GetUserByEmailAsync.
        }

        /// <summary>
        /// Método para criar utilizadores
        /// </summary>
        /// <param name="user">Utilizador a ser criado</param>
        /// <param name="password">Palavra-passe para o utilizador</param>
        /// <returns>Resultado da operação</returns>
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }


        /// <summary>
        /// Método para obter user pelo email
        /// </summary>
        /// <param name="email">Endereço de e-mail do utilizador</param>
        /// <returns>Utilizador correspondente, ou null</returns>
        public async Task<User> GetUserByEmailAsync(string email) //Procura um utilizador na base de dados através do e-mail (usado como identificador).
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
