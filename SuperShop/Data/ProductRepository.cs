using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;

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
        private readonly DataContext _context;

        public ProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os produtos existentes na base de dados, incluindo os respetivos utilizadores associados.
        /// </summary>
        /// <returns>Uma coleção do tipo <see cref="IQueryable"/> que contém os produtos,
        /// cada um com os dados do utilizador associado</returns>
        /// /// <remarks>
        /// Utiliza <c>Include(p => p.User)</c> para incluir a entidade relacionada <c>User</c>
        /// no resultado da consulta, evitando múltiplas queries à base de dados.
        /// </remarks>
        public IQueryable GetAllWithUsers()   //O "IQueryable" devolve uma coleção de elementos consultável na base de dados.
        {
            return _context.Products.Include(p => p.User);  //Dá-me os produtos com o "join" do utilizador
        }
    }
}
