using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();   //Vê se a Base de Dados está criada (se não estiver criada, cria-a)

            //Cria os produtos
            if(!_context.Products.Any())    //Se não tem lá produtos
            {
                AddProduct("IPhone X");
                AddProduct("Magic Mouse");
                AddProduct("iwatch Series 4");
                AddProduct("ipad Mini");
                await _context.SaveChangesAsync();  //Gravo o produto na base de dados
            }
        }

        private void AddProduct(string name)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),  //Cria um preço fictício até mil
                IsAvailable = true,
                Stock = _random.Next(100)

            });
        }
    }
}
