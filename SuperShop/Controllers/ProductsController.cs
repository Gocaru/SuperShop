using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        //private readonly DataContext _context;  //vai buscar o DataContext

        //public ProductsController(DataContext context)
        //{
        //    _context = context; //Injeta o DataContext
        //}

        //private readonly IRepository _repository;

        //public ProductsController(IRepository repository)
        //{
        //    _repository = repository;
        //}

        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        //public ProductsController(IProductRepository productrepository)
        //{
        //    _productRepository = productrepository;
        //}

        public ProductsController(
            IProductRepository productRepository,
            IUserHelper userHelper) //Injeto o IUserHelper
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        //// GET: Products
        ////Eu não quero isto. Não quero que o controlador tenha acesso direto à tabela.
        ////Por isso vou utilizar o Pattern Repositório
        //public async Task<IActionResult> Index()
        //{
        //    // Obtém todos os produtos da base de dados de forma assíncrona
        //    // e envia a lista de produtos para a view "Index".
        //    return View(await _context.Products.ToListAsync());
        //}

        // GET: Products
        //public IActionResult Index()
        //{
        //    return View(_repository.GetProducts()); 
        //}
        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));  //Aqui é o único sítio que posso ordenar pelo nome, pois pode haver entidades que não rpecisem de "Name" como propreidade
                                                                            //(se todas precisassem, deveria colocar no IEntity)
        }

        //// GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)   //Passo o Id do produto que quero ver (opcional, pois tem um ponto de exclamação)
        //{
        //    //Se o Id não existir:
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .FirstOrDefaultAsync(m => m.Id == id);  //Vai buscar o produto com o Id passado como parâmetro 
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    //Se encontrar:
        //    return View(product);
        //}


        //// GET: Products/Details/5
        //public IActionResult Details(int? id) 
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _repository.GetProduct(id.Value);  //Vai buscar o produto com o Id passado como parâmetro utilizando o método "GetProduct" criado na classe Repository
        //                                                     //Coloco "Id.Value" para a aplicação não rebentar se não for passado nenhum "id" (vai passar um valor nulo)
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);  
                                                             
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        //Tenho dois creates: um com o GET e um com o POST:
        //O Get apenas abre a View do create
        //O Post é responsável por receber o modelo e mandar para a base de dados
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Products/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product)
        ////Altero "public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)"
        ////Para "public async Task<IActionResult> Create(Product product)"  //É para descomplicar
        //{
        //    if (ModelState.IsValid) //Vê se o produto é válido, ou seja, se cumpre as regras que estão na classe Product
        //    {
        //        _context.Add(product);  //Adiciona o produto na memória
        //        await _context.SaveChangesAsync();  //Grava o produto de forma assíncrona, para o utilizador poder continuar a interagir com o programa 
        //        return RedirectToAction(nameof(Index)); //Redireciona para a action "Index", onde é mostrada a lista dos produtos
        //    }
        //    return View(product);   //Se ocorrer algum problema, mostra os dados inseridos do produto, para que o utilizador não tenha de escrever novamente os campos
        //}

       // //POST: Products/Create
       //[HttpPost]
       //[ValidateAntiForgeryToken]
       // public async Task<IActionResult> Create(Product product)
       // {
       //     if (ModelState.IsValid)
       //     {
       //         _repository.AddProduct(product);
       //         await _repository.SaveAllAsync();
       //         return RedirectToAction(nameof(Index));
       //     }
       //     return View(product);
       // }

        // POST: Products/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //ToDo: Modificar para o user que tiver logado
        //        product.User = await _userHelper.GetUserByEmailAsync("goncalorusso@gmail.com");
        //        await _productRepository.CreateAsync(product);  //Chama o método "CreateAsync" do repositório para adicionar o objeto "product" à base de dados.
        //                                                        //Este método está implementado no "GenericRepository<Product>", herdado por "ProductRepository".
        //                                                        //O produto é tratado como uma entidade genérica "T" e adicionado ao contexto através de "AddAsync" (EF Core).
        //                                                        //A gravação é concluída dentro do próprio método "CreateAsync", que invoca "SaveAllAsync",
        //                                                        //pelo que não é necessário gravar manualmente no controlador.


        //        //await _productRepository.SaveAllAsync();  //Não é necessário chamar este método no controlador, pois já é executado dentro de "CreateAsync".
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Coloco as imagens
                var path = string.Empty; //Caminho da imagem

                if(model.ImageFile != null && model.ImageFile.Length > 0)  //Se foi carregada uma imagem
                {
                    //Para não correr o risco de ter dois ficheiros de imagem com o mesmo nome:
                    var guid = Guid.NewGuid().ToString();   //Crio um objeto do tipo "Guid" (identificador único) e converto para string para o poder guardar
                    var file = $"{guid}.jpg";   //Converto para imagem .jpg

                    //Construo o caminho para onde vai ser gravada
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),    //Indico o sítio onde vou gravar
                        "wwwroot\\images\\products",        //E o caminho para a pasta respetiva
                                                            //model.ImageFile.FileName);          //Vou buscar o resto do ficheiro
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))         //Vou gravar
                    {
                        await model.ImageFile.CopyToAsync(stream);      //Aqui guarda
                    }

                    //Indico o caminho da Base de Dados
                    //path = $"~/images/products/{model.ImageFile.FileName}";
                    path = $"~/images/products/{file}";
                }

                //Antes de gravar para a BD vou ter de converter o ProductViewModel num Product (pois o que quero gravar na tabela é um Product)
                var product = this.ToProduct(model, path);  //Vou buscar o método que vai fazer essa conversão ("ToProduct")


                //ToDo: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync("goncalorusso@gmail.com");
                await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        /// <summary>
        /// Converte um objeto do tipo "ProductViewModel" num objeto "Product"
        /// </summary>
        /// <param name="model">A ViewModel que contém os dados do produto a converter</param>
        /// <param name="path">O caminho da imagem que será atribuído à propriedade ImageUrl</param>
        /// <returns>Um novo objeto <see cref="Product"/> com os dados preenchidos a partir da ViewModel</returns>
        /// <remarks>
        /// Este método é útil para transformar os dados recebidos do formulário (ViewModel) num objeto do modelo de domínio
        /// que possa ser persistido na base de dados.
        /// </remarks>
        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        //// GET: Products/Edit/5
        ///// <summary>
        ///// Permite editar um produto existente
        ///// </summary>
        ///// <param name="id">Identificador do produto a editar. Pode ser nulo.</param>
        ///// <returns>Retorna a view de edição com o produto se encontrado; caso contrário, retorna "NotFound".</returns>
        //public async Task<IActionResult> Edit(int? id)  //No Edit tenho o "int" com um ponto de interrogação (um "nullable int"), pois o id pode ou não ser fornecido.
        //                                                //Tornar o "id" nullable evita que a aplicação lance exceções se o parâmetro não for passado.
        //{
        //    if (id == null)
        //    {
        //        // Se não for fornecido nenhum id, retorna a página de erro "NotFound".
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);  // Procura na base de dados um produto com o id fornecido.
        //    if (product == null)
        //    {
        //        // Se não encontrar o produto, também retorna "NotFound".
        //        return NotFound();
        //    }
        //    // Se encontrar o produto, apresenta-o na view de edição.
        //    return View(product);
        //}

        //// GET: Products/Edit/5
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _repository.GetProduct(id.Value); 
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        // GET: Products/Edit/5
        //public async  Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _productRepository.GetByIdAsync(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var model = this.ToProductViewModel(product);
            return View(model);
        }

        /// <summary>
        /// Converte um objeto do tipo <see cref="Product"/> para um objeto <see cref="ProductViewModel"/>.
        /// </summary>
        /// <param name="product">O objeto <see cref="Product"/> a ser convertido para ViewModel, normalmente obtido a partir da base de dados.</param>
        /// <returns>Um novo objeto <see cref="ProductViewModel"/> com os dados preenchidos a partir do modelo de domínio <c>Product</c>,
        /// preparado para ser apresentado na interface.</returns>
        /// /// <remarks>
        /// Este método é útil quando se pretende enviar dados para uma View que depende de uma estrutura personalizada,
        /// como é o caso das operações de criação ou edição de produtos com campos adicionais ou formatação específica.
        /// </remarks>
        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        //// POST: Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ///// <summary>
        ///// Processa a submissão do formulário de edição de um produto.
        ///// Atualiza os dados do produto na base de dados se as informações forem válidas.
        ///// </summary>
        ///// <param name="id">Identificador do produto a editar. Deve corresponder ao Id do objeto recebido.</param>
        ///// <param name="product">Objeto Product com os novos dados a serem atualizados.</param>
        ///// <returns>Redireciona para a lista de produtos (Index) se a edição for bem-sucedida; 
        ///// caso contrário, retorna a view de edição com os dados atuais e mensagens de validação.</returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        ////Altero "public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)"
        ////Para "public async Task<IActionResult> Edit(int id, Product product)"  //É para descomplicar
        //{
        //    //Vejo se o Id existe
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);   //Faço o update do produto
        //            await _context.SaveChangesAsync();  //Gravo
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            //Se o produto não existia
        //            if (!ProductExists(product.Id)) //Por exemplo, se o produto for eliminado por outro utilizador, enquanto o estiver a tentar editar
        //            {
        //                return NotFound();
        //            }
        //            //Ou outra situação
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);   //retrono a mesma view com o produto lá dentro
        //}

        //// POST: Products/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UpdateProduct(product);   //Faço o update do produto utilizando o método "UpdateProduct()"
        //            await _repository.SaveAllAsync();  //Gravo utilizando o "SaveAllAsync()"
        //        }
        //        catch (DbUpdateConcurrencyException) //Se dois utilizadores tentarem alterar o mesmo registo ao mesmo tempo na base de dados
        //        {
        //            if (!_repository.ProductExists(product.Id)) //Se, utilizando o método "ProductExists", não existe o Id do produto na BD
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}

        // POST: Products/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //ToDo: Modificar para o user que tiver logado
        //            product.User = await _userHelper.GetUserByEmailAsync("goncalorusso@gmail.com"); // Garante que, ao atualizar o produto, o campo User associado ao mesmo não fica nulo e está corretamente ligado a um utilizador existente.
        //            await _productRepository.UpdateAsync(product);   //Faço o update do produto utilizando o método "UpdateAsync()"
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (! await _productRepository.ExistAsync(product.Id)) //Se, utilizando o método "ExistAsync", não existe o Id do produto na BD
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    //Para o caso de não alterar a imagem
                    var path = model.ImageUrl;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        //Para não correr o risco de ter dois ficheiros de imagem com o mesmo nome:
                        var guid = Guid.NewGuid().ToString();   //Crio um objeto do tipo "Guid" (identificador único) e converto para string para o poder guardar
                        var file = $"{guid}.jpg";   //Converto para imagem .jpg


                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\products",
                            //model.ImageFile.FileName);
                            file);

                        using(var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        //path = $"~/images/products/{model.ImageFile.FileName}";
                        path = $"~/images/products/{file}";
                    }

                    var product = this.ToProduct(model, path);

                    //ToDo: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("goncalorusso@gmail.com"); // Garante que, ao atualizar o produto, o campo User associado ao mesmo não fica nulo e está corretamente ligado a um utilizador existente.
                    await _productRepository.UpdateAsync(product);   //Faço o update do produto utilizando o método "UpdateAsync()"
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id)) //Se, utilizando o método "ExistAsync", não existe o Id do model na BD
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //// GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)    //Mostra o que é para apagar
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// GET: Products/Delete/5
        //public IActionResult Delete(int? id)    //Mostra o que é para apagar
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = _repository.GetProduct(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)    //Mostra o que é para apagar
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]  //Quando existir uma action delete mas feita com um Post, é feito o DeleteConfirmed (estou a fazer um reecaminhamento)
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)        //É o que apaga efetivamente
        //{
        //    var product = await _context.Products.FindAsync(id);    //Vou ainda à tabela para ver se ainda lã está (pode ter sido apagado entretanto por outro utilizador)
        //    _context.Products.Remove(product);      //Remove em memória
        //    await _context.SaveChangesAsync();      //Apaga na Base de dados
        //    return RedirectToAction(nameof(Index));
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")] 
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)       
        //{
        //    var product = _repository.GetProduct(id);    //Não ponho aqui i "id.Value" pois o campo do "id" não é opcional
        //    _repository.RemoveProduct(product);      
        //    await _repository.SaveAllAsync();     
        //    return RedirectToAction(nameof(Index));
        //}

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)      
        {
            var product = await _productRepository.GetByIdAsync(id);   
            await _productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)      //Método auxiliar para saber se o produto existe ou não
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
