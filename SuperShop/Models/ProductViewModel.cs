using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SuperShop.Data.Entities;


namespace SuperShop.Models
{
    public class ProductViewModel : Product //A view vai ter como modelo o "ProductViewModel", e não o "Product"
                                            //Não vai para a base de dados
    {
        //Vou ter todos os campos do "Product" mais a imagem que quero carregar
        [Display (Name = "Image")]
        public IFormFile ImageFile { get; set; }  //vou ter um tipo novo, o "IFormFile", que é só da ViewModel
}
}
