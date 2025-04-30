using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    /// <summary>
    /// Interface que define um repositório genérico para operações de acesso a dados sobre entidades do tipo "T"
    /// </summary>
    /// <typeparam name="T">ipo da entidade a ser gerida pelo repositório. Deve ser uma classe.</typeparam>
    public interface IGenericRepository<T> where T : class  //Interface que vai criar o repositório genérico
                                                            //Vai receber uma Classe Entidade. Identifico a classe a receber como "T"
                                                            //(uma classe entidade é uma classe especial que representa uma tabela de base de dados)
                                                            //O "T" significa que vai entrar alguma coisa genérica
    {
        //Faço o CRUD

        //1 - Método para obter todas as entidades existentes.
        IQueryable<T> GetAll(); // Devolve todas as entidades do tipo "T".

        //2 - Método para obter uma entidade pelo seu identificador (id) de forma assíncrona.
        Task<T> GetByIdAsync(int id); //O Id recebido é a única propriedade comum a todas as entidades (está no IEntity)

        //3 - Método para criar a entidade
        Task CreateAsync(T entity);

        //4 - Método para fazer o updade da entidade
        Task UpdateAsync(T entity);

        //4 - Método para eliminar entidade
        Task DeleteAsync(T entity);

        //5 - Método auxiliar para ver se a entidade existe
        Task<bool> ExistAsync(int id);

        //Não coloco um método genérico para gravar, pois gravar implica ter acesso à tabela
        //Isso vai ser feito na classe
    }
}
