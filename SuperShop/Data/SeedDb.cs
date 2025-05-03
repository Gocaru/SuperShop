using System;
using System.Linq;
using SuperShop.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Helpers;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        //private readonly UserManager<User> _userManager;
        private Random _random;

        //public SeedDb(DataContext context, UserManager<User> userManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    _random = new Random();   //Gera dados fictícios durante o processo de "seeding"
        //}

        //Em vez de usar o UserManager vou usar o UserHelper
        /// <summary>
        /// Construtor da classe "SeedDb" responsável por preparar os dados iniciais da base de dados.
        /// </summary>
        /// <param name="context">Instância do "DataContext" utilizada para aceder à base de dados.</param>
        /// <param name="userHelper">nstância da interface "IUserHelper"/ que fornece métodos auxiliares para criar e gerir utilizadores.</param>
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        //public async Task SeedAsync()
        //{
        //    await _context.Database.EnsureCreatedAsync();   //Vê se a Base de Dados está criada (se não estiver criada, cria-a)

        //    var user = await _userManager.FindByEmailAsync("goncalorusso@gmail.com");  //Vejo se já existe este user

        //    //Se não existir:
        //    if (user == null)
        //    {
        //        user = new User
        //        {
        //            FirstName = "Gonçalo",
        //            LastName = "Russo",
        //            Email = "goncalorusso@gmail.com",
        //            UserName = "goncalorusso@gmail.com"
        //            //Não tenho aqui uma propriedade chamada password (nunca está no objeto, para que possa depois ser encriptada). 
        //        };
        //        //Utilizo a classe UserManager para criar o utilizador e indicar a password
        //        var result = await _userManager.CreateAsync(user, "123456");

        //        //Vejo se criou ou não:
        //        if (result != IdentityResult.Success)
        //        {
        //            throw new InvalidOperationException("Couldnot create the user in seeder");
        //        }
        //    }


        //    //Cria os produtos
        //    if (!_context.Products.Any())    //Se não tem lá produtos
        //    {
        //        AddProduct("IPhone X", user); //Quando crio os produtos, tenho de indicar o utilizador que os criou, por isso passo o parâmatero "user"
        //        AddProduct("Magic Mouse", user);
        //        AddProduct("iwatch Series 4", user);
        //        AddProduct("ipad Mini", user);
        //        await _context.SaveChangesAsync();  //Gravo o produto na base de dados
        //    }
        //}

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();   //Vê se a Base de Dados está criada (se não estiver criada, cria-a)

            var user = await _userHelper.GetUserByEmailAsync("goncalorusso@gmail.com");  //Vejo se já existe este user

            //Se não existir:
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Gonçalo",
                    LastName = "Russo",
                    Email = "goncalorusso@gmail.com",
                    UserName = "goncalorusso@gmail.com"
                    //Não tenho aqui uma propriedade chamada password (nunca está no objeto, para que possa depois ser encriptada). 
                };
                //Utilizo a classe UserManager para criar o utilizador e indicar a password
                var result = await _userHelper.AddUserAsync(user, "123456");

                //Vejo se criou ou não:
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Couldnot create the user in seeder");
                }
            }


            //Cria os produtos
            if (!_context.Products.Any())    //Se não tem lá produtos
            {
                AddProduct("IPhone X", user); //Quando crio os produtos, tenho de indicar o utilizador que os criou, por isso passo o parâmatero "user"
                AddProduct("Magic Mouse", user);
                AddProduct("iwatch Series 4", user);
                AddProduct("ipad Mini", user);
                await _context.SaveChangesAsync();  //Gravo o produto na base de dados
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),  //Cria um preço fictício até mil
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
