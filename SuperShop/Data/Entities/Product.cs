using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    //Vai dar origem a uma tabela na Base de dados:
    public class Product
    {
        //[Key]      //Tb poderia fazer uma Data Anotation ("[Key]") para indicar que o campo vai ser chave primária
        public int Id { get; set; }  //Por defeito deteta que é a chave primária pois é "Id"

        public string Name { get; set; }


        //Indico a formatação do "Price". "C" significa currency; formato em duas casas decimais ("{0:C2}") me modo currency; mas quando estiver em modo de edição não faz formato específico ("ApplyFormatInEditMode = false")
        //Quando estiver na base de dados e na aplicação web, está com este formato
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]     //Faço esta Data Anotation para aparecer na webpage, como campo, "Image" e não "ImageUrl"
        public string ImageUrl { get; set; }  //Link da imagem

        [Display(Name = "Last Purchase")]
        public DateTime LastPurchase { get; set; }


        [Display(Name = "Last Sale")]
        public DateTime LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }


    }
}
