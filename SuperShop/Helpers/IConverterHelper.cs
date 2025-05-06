using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;

namespace SuperShop.Helpers
{
    /// <summary>
    /// Define os métodos de conversão entre Product e ProductViewModel
    /// </summary>
    /// <remarks>
    /// Separa as responsabilidades e mantém o código limpo: as Views lidam com ViewModels, e a base de dados com Models
    /// </remarks>
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, string path, bool isNew); //Converte um ProductViewModel para Product (pronto para guardar na base de dados).
                                                                            //O "model" é o objeto da View com os dados introduzidos pelo utilizador
                                                                            //A "path" representa o caminho (URL ou relativo) da imagem do produto.
                                                                            //O "IsNew" indica se se trata de um novo produto (true) ou de um produto que está a ser editado (false):
                                                                            //Se for novo, o Id pode ser deixado em branco.
                                                                            //Se for uma edição, o Id do produto deve ser mantido.

        ProductViewModel ToProductViewModel(Product product); //Converte um Product da base de dados num ProductViewModel, para ser apresentado na View (por exemplo, num formulário de edição).
    }
}
