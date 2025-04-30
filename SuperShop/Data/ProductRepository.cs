using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    /// <summary>
    /// Repositório específico da entidade Product
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository   //Herda toda a funcionalidade genérica de acesso a dados da classe "GenericRepository<Product>",
                                                                                      //(como os métodos para criar, atualizar, eliminar, obter por ID e listar todos os registos).
                                                                                      //Implementa também a interface "IProductRepository", que herda de "IGenericRepository<Product>",
                                                                                      //E implementa IProductRepository (obriga a classe ProductRepository a cumprir a interface específica de produtos)
                                                                                      //(permitindo especializar e estender futuramente o repositório com métodos próprios da entidade Product).
                                                                                      //Vai buscar o "Product" e implementa o Interface IProductsRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
        }
    }
}
