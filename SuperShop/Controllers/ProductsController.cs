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
        private readonly DataContext _context;  //vai buscar o DataContext

        public ProductsController(DataContext context)
        {
            _context = context; //Injeta o DataContext
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync()); //Retorna a view e vai buscar todos os produtos que lá estão
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)   //Passo o Id do produto que quero ver
        {
            //Se o Id não existir:
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);  //Vai buscar o produto com o Id passado como parâmetro
            if (product == null)
            {
                return NotFound();
            }

            //Se encontrar:
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

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        //Altero "public async Task<IActionResult> Create([Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)"
        //Para "public async Task<IActionResult> Create(Product product)"  //É para descomplicar
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        //Altero "public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)"
        //Para "public async Task<IActionResult> Edit(int id, Product product)"  //É para descomplicar
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)        //É o que apaga efetivamente
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);      //Remove em memória
            await _context.SaveChangesAsync();      //Apaga na Base de dados
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)      //Método auxiliar para saber se o produto existe ou não
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
