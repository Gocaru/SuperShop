using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    //public class DataContext : DbContext
    public class DataContext : IdentityDbContext<User>    //Porque agora tenho utilizadores, e em consequência vou precisar de um sistema de autenticação,
                                                          //o DataContext vai herdar do IdentityDbContext (responsável pela autenticação).
                                                          //Injeto o meu "User" para saber que vai receber um objeto User.
    {
        /// <summary>
        /// Representa a coleção de produtos (tabela "Products") no contexto da base de dados.
        /// </summary>
        public DbSet<Product> Products { get; set; }   //Esta propriedade estará ligada à tabela "Products" na base de dados.

        /// <summary>
        /// Construtor do DataContext que recebe as opções de configuração do Entity Framework Core.
        /// </summary>
        /// <param name="options">Objeto do tipo DbContextOptions contendo as configurações necessárias para estabelecer a ligação à base de dados.</param>
        public DataContext(DbContextOptions<DataContext> options) : base (options)   // Este construtor recebe as informações necessárias para ligar o programa à base de dados.
                                                                                     // Essas informações são enviadas para esta classe automaticamente (injeção de dependência).
                                                                                     // O DataContext aproveita tudo o que a classe DbContext já faz, como ligar e trabalhar com a base de dados.
        {
        }
    }
}
