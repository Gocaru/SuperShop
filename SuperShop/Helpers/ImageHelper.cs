using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    /// <summary>
    /// Classe auxiliar responsável por operações relacionadas com o carregamento de imagens para o servidor.
    /// </summary>
    /// <remarks>
    /// Implementa a interface <see cref="IImageHelper"/> e fornece funcionalidade para gravar ficheiros enviados via formulário
    /// na estrutura de pastas da aplicação (dentro de <c>wwwroot/images</c>), devolvendo o caminho virtual para posterior utilização na interface.
    /// </remarks>
    public class ImageHelper : IImageHelper
    {

        /// <summary>
        /// Carrega um ficheiro de imagem enviado via formulário para a pasta especificada dentro de <c>wwwroot/images</c>
        /// e devolve o caminho virtual da imagem guardada.
        /// </summary>
        /// <param name="imageFile">O ficheiro de imagem enviado através do formulário</param>
        /// <param name="folder">A subpasta dentro de <c>wwwroot/images</c> onde a imagem será armazenada.</param>
        /// <returns>O caminho virtual da imagem</returns>
        /// <remarks>
        /// Um nome de ficheiro único é gerado automaticamente com <see cref="Guid"/> para evitar conflitos.
        /// O ficheiro é guardado com a extensão ".jpg". O método deve ser usado em contextos onde seja necessário guardar imagens submetidas por utilizadores.
        /// </remarks>
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            string guid = Guid.NewGuid().ToString();   //Gera um identificador único (GUID) para garantir que o nome do ficheiro será sempre único, evitando conflitos com ficheiros já existentes.
                                                       //Converto para string para o poder guardar.
            string file = $"{guid}.jpg";    //Cria o nome do ficheiro final 

            //Cria o caminho físico completo para onde o ficheiro será guardado no disco.
            //Usa Directory.GetCurrentDirectory() para obter a raiz da aplicação.
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\images\\{folder}",
                file);

            //Grava a imagem enviada pelo formulário num ficheiro físico no disco do computador (dentro da pasta wwwroot do projeto).
            using (FileStream stream = new FileStream(path, FileMode.Create))   //O FileStream é um "canal" que permite escrever num ficheiro
                                                                                //O ficheiro vai ser guardado no "path"
                                                                                //"FileMode.Create" diz: "Cria um novo ficheiro — se já existir, substitui-o".
            {
                await imageFile.CopyToAsync(stream);    //Copia o conteúdo do imageFile para dentro do FileStream, ou seja, grava-o no disco.
            }

            return $"~/images/{folder}/{file}"; //Devolve o caminho virtual da imagem, que pode ser usado numa View Razor.
        }
    }
}
