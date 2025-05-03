using Microsoft.AspNetCore.Identity;

namespace SuperShop.Data.Entities
{
    public class User : IdentityUser    //A classe User herda da classe IdentityUser (que é uma classe do ASP.NET Core) que já tem uma série de propriedades 
    {
        //Aqui vou acrescentar propriedades que não estão definidas por default no IdentityUser, mas que são importantes para o meu projeto
        public string FirstName { get; set; }  
        
        public string LastName { get; set; }
    }
}
