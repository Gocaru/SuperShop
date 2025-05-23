﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    /// <summary>
    /// Representa um produto no sistema da loja. Esta classe corresponde a uma tabela na base de dados.
    /// </summary>
    public class Product : IEntity  //Implementa IEntity (possui uma chave primária chamada "Id")
    {
        //[Key]      //Tb poderia fazer uma Data Anotation ("[Key]") para indicar que o campo vai ser chave primária
        public int Id { get; set; }  //Por defeito deteta que é a chave primária pois é "Id"

        [Required]  //Torna o campo obrigatório de preenchimmento
        [MaxLength(50, ErrorMessage ="The filed {0} can contain {1} characters length")]  //O campo "Name" só pode conter 50 caracteres
        public string Name { get; set; }


        //Indico a formatação do "Price". "C" significa currency; formato em duas casas decimais ("{0:C2}") me modo currency; mas quando estiver em modo de edição não faz formato específico ("ApplyFormatInEditMode = false")
        //Quando estiver na base de dados e na aplicação web, está com este formato
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]     //Faço esta Data Anotation para aparecer na webpage, como campo, "Image" e não "ImageUrl"
        public string ImageUrl { get; set; }  //Link da imagem


        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; } //Torno o DateTime opcional (coloquei um ponto de interrogação)


        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }   //Torno o DateTime opcional (coloquei um ponto de interrogação)

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        //O meu produto tb vai ter de ter uma propriedade que é o "User" (pois quero saber o utilizador que inseriu um determinado produto)
        public User User { get; set; }  //Cria uma relação de um para muitos (1 user pode ter muitos produtos, mas um produto só pode ter 1 user)

        //propriedade que tem como objetivo construir e devolver o caminho completo (URL absoluto) de uma imagem, a partir do seu caminho relativo armazenado em ImageUrl.
        public string ImageFullPath
        {
            get
            {
                if(string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return $"https://localhost:44393{ImageUrl.Substring(1)}";
            }
        }
    }
}
