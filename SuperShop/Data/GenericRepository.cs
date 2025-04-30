using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;
using System.Threading.Tasks;


namespace SuperShop.Data
{
    /// <summary>
    /// Implementa um repositório genérico para aceder a entidades do tipo "T"
    /// </summary>
    /// <typeparam name="T">A entidade a ser gerida pelo repositório, que deve ser uma classe que implementa "IEntity"</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity //No "GenericRepository" vou implementar o Interface genérico ("IGenericRepository")
                                                                                       //Restringe o tipo "T" para ser uma classe que implementa "IEntity".
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        //Método para ir buscar todas as entidades do tipo "T"
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();    //Vai buscar todos os objetos do tipo "T" (entidade que implementa "IEntity").
                                                        //Vai à tabela de "T" e traz tudo o que lá esteja ("Set<T>")
                                                        //Utiliza "AsNoTracking()" para melhorar o desempenho, não mantendo o controlo das entidades carregadas.
        }

        //Método para obter uma entidade do tipo "T" pelo seu identificador.
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()              //Acede à tabela correspondente ao tipo "T".
                .AsNoTracking()                         //Lê os dados sem associá-los ao contexto ("AsNoTracking()").
                .FirstOrDefaultAsync(e => e.Id == id);  //Localiza a primeira entidade cujo "Id" corresponde ao valor fornecido.
        }

        //Método para criar uma entidade
        public async Task CreateAsync(T entity)  //Recebe uma entidade do tipo "T"
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }
        //Crio um método ("CreateAsync()") que na prática tem como única função chamar outro ("AddAsync()")

        //Método para editar uma entidade
        public async Task UpdateAsync(T entity)  //Recebe uma entidade do tipo "T"
        {
            _context.Set<T>().Update(entity);   //O "Update" não é async
            await SaveAllAsync();
        }

        //Método para eliminar uma entidade
        public async Task DeleteAsync(T entity)  //Recebe uma entidade do tipo "T"
        {
            _context.Set<T>().Remove(entity);   //O "Remove" tb não é async
            await SaveAllAsync();
        }

        //Método auxiliar para ver se a entidade existe
        public async Task<bool> ExistAsync(int id)  //Recebe um id
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //O "SaveChangesAsync()" grava tudo o que está pendente para a BD
                                                          //(executa o INSERT, UPDATE ou DELETE correspondentes na base de dados)
                                                          //O "SaveChangesAsync()" devolve um número inteiro que indica quantas linhas foram afetadas na base de dados.
                                                          //Se o resultado for maior que 0, significa que foram feitas alterações (pelo menos uma linha foi inserida, alterada ou removida).
                                                          //Se for 0, significa que não houve nada para gravar.
                                                          //O "return" pode ser true (se houve alterações salvas na base de dados) ou false (se não houve alterações).
        }
    }

}
