using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {


        //private readonly DataContext _context;  //vai buscar o DataContext

        //public ProductsController(DataContext context)
        //{
        //    _context = context; //Injeta o DataContext
        //}

        private readonly IRepository _repository;

        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }


        ////Eu não quero isto. Não quero que o controlador tenha acesso direto à tabela.
        ////Por isso vou utilizar o Pattern Repositório
        //// GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    // Obtém todos os produtos da base de dados de forma assíncrona
        //    // e envia a lista de produtos para a view "Index".
        //    return View(await _context.Products.ToListAsync());
        //}

        public IActionResult Index()
        {
            return View(_repository.GetProducts()); 
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


        // GET: Products/Details/5
        public IActionResult Details(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);  //Vai buscar o produto com o Id passado como parâmetro utilizando o método "GetProduct" criado na classe Repository
                                                             //Coloco "Id.Value" para a aplicação não rebentar se não for passado nenhum "id" (vai passar um valor nulo)
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Tenho dois creates: um com o GET e um com o POST:
        //O Get apenas abre a View do create
        //O Post é responsável por receber o modelo e mandar para a base de dados

        // GET: Products/Create
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.AddProduct(product);
                await _repository.SaveAllAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        ///// <summary>
        ///// Permite editar um produto existente
        ///// </summary>
        ///// <param name="id">Identificador do produto a editar. Pode ser nulo.</param>
        ///// <returns>Retorna a view de edição com o produto se encontrado; caso contrário, retorna "NotFound".</returns>
        //// GET: Products/Edit/5
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

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value); 
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        ///// <summary>
        ///// Processa a submissão do formulário de edição de um produto.
        ///// Atualiza os dados do produto na base de dados se as informações forem válidas.
        ///// </summary>
        ///// <param name="id">Identificador do produto a editar. Deve corresponder ao Id do objeto recebido.</param>
        ///// <param name="product">Objeto Product com os novos dados a serem atualizados.</param>
        ///// <returns>Redireciona para a lista de produtos (Index) se a edição for bem-sucedida; 
        ///// caso contrário, retorna a view de edição com os dados atuais e mensagens de validação.</returns>
        //// POST: Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdateProduct(product);   //Faço o update do produto utilizando o método "UpdateProduct()"
                    await _repository.SaveAllAsync();  //Gravo utilizando o "SaveAllAsync()"
                }
                catch (DbUpdateConcurrencyException) //Se dois utilizadores tentarem alterar o mesmo registo ao mesmo tempo na base de dados
                {
                    if (!_repository.ProductExists(product.Id)) //Se, utilizando o método "ProductExists", não existe o Id do produto na BD
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
            return View(product);
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

        public IActionResult Delete(int? id)    //Mostra o que é para apagar
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value);
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

        [HttpPost, ActionName("Delete")]  //Quando existir uma action delete mas feita com um Post, é feito o DeleteConfirmed (estou a fazer um reecaminhamento)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)        //É o que apaga efetivamente. 
        {
            var product = _repository.GetProduct(id);    //Não ponho aqui i "id.Value" pois o campo do "id" não é opcional
            _repository.RemoveProduct(product);      
            await _repository.SaveAllAsync();     
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)      //Método auxiliar para saber se o produto existe ou não
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
