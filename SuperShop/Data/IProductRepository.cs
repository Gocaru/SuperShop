using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    /// <summary>
    /// Define o contrato para o repositório da entidade "Product" (especifica as operações permitidas sobre os dados da entidade Product, sem se preocupar com a implementação concreta)
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>   // Herda da interface genérica "IGenericRepository<Product>", o que significa que esta interface já inclui os métodos 
                                                                        // de acesso genéricos à base de dados (como GetAll, GetByIdAsync, CreateAsync, etc.).
                                                                        // Esta interface serve como ponto de especialização, permitindo futuramente adicionar métodos específicos 
                                                                        // relacionados com produtos (por exemplo, GetByCategoryAsync), mantendo a estrutura limpa e extensível.
    {
    }
}
