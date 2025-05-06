using System.Linq;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    /// <summary>
    /// Define o contrato para o repositório da entidade "Product"
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>   // Herda da interface genérica "IGenericRepository<Product>", o que significa que esta interface já inclui os métodos 
                                                                        // de acesso genéricos à base de dados (como GetAll, GetByIdAsync, CreateAsync, etc.).
                                                                        // É uma interface sem implementação de código
                                                                        // (serve como contrato: define que um repositório de produtos tem de implementar os métodos do IGenericRepository<Product>).
                                                                        //Ao não implementar código, vai indicar ao sistema a necessidade de indicar quem vai implementar o contrato
                                                                        //E esta linha de código é cumprida: "services.AddScoped<IProductRepository, ProductRepository>();"
                                                                        //Mas o ProductRepository não implementa diretamente os métodos do repositório,
                                                                        //contudo herda do "GenericRepository<Product>" onde estão todos os métodos (CreateAsync, GetAll, etc.).
                                                                        //Dentro do "CreateAsync" está o AddAsync(entity), que é um método do Entity Framework Core, que adiciona a entidade ao contexto (DbContext).
                                                                        //Em resumo o caminho completo:
                                                                        //Controller
                                                                        //    ↓
                                                                        //IProductRepository
                                                                        //    ↓ (ligado por AddScoped)
                                                                        //ProductRepository
                                                                        //    ↓ (herda de)
                                                                        //GenericRepository<Product>
                                                                        //    ↓
                                                                        //CreateAsync(product)
                                                                        //    ↓
                                                                        //AddAsync(product)  ← método do Entity Framework Core

    {
        public IQueryable GetAllWithUsers();
    }
}
