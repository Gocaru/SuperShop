using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Crio a action que me vai dar todos os produtos
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAll());  //Retorna todos os produtos do repositório
                                                     //O "Ok" embrulha tudo dentro de um Json
        }

    }
}
