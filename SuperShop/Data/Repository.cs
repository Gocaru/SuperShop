using Microsoft.AspNetCore.Routing;
using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SuperShop.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)  //Injeto o DataContext no CTOR para ter acesso à Base de Dados através da cdesta classe
        {
            _context = context;
        }

        //Faço um CRUD inteiro

        //Crio um método que me dê todos os produtos (READ do CRUD)
        public IEnumerable<Product> GetProducts() //Injeto um Product
        {
            //Vou buscar todos os produtos
            return _context.Products.OrderBy(p => p.Name);  //Ordeno pelo nome do produto
        }

        //Método que me dá apenas um produto (segundo READ do CRUD)
        public Product GetProduct(int id)
        {
            return _context.Products.Find(id);
        }

        //Método para adicionar um produto (é o CREATE do CRUD)
        public void AddProduct(Product product)
        {
            _context.Products.Add(product); //Adiciono o produto em memória (posteriormente vou criar um método para adicionar na Base de Dados)
        }

        //Método para fazer o UPDATE
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        //Metodo para fazer o DELETE
        public void RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        //Métodos auxiliares:
        //1 - Método para gravar para a BD
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;   //O "SaveChangesAsync()" grava tudo o que está pendente para a BD
                                                            //(executa o INSERT, UPDATE ou DELETE correspondentes na base de dados)
                                                            //O "SaveChangesAsync()" devolve um número inteiro que indica quantas linhas foram afetadas na base de dados.
                                                            //Se o resultado for maior que 0, significa que foram feitas alterações (pelo menos uma linha foi inserida, alterada ou removida).
                                                            //Se for 0, significa que não houve nada para gravar.
                                                            //O "return" pode ser true (se houve alterações salvas na base de dados) ou false (se não houve alterações). 
        }

        //2 - Método para saber se o produto existe
        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);  //Não devolve um produto. Vai ver se existe ou não e devolve um bool.
        }

    }
}
