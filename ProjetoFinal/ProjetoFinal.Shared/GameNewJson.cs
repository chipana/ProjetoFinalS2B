using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ProjetoFinal
{
    /// <summary>
    /// Classe criada para agregar os dados que vem do Json, quando pesquisa por mais noticias.
    /// </summary>
    [JsonObject]
    class GameNewJson
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("contents")]
        public string Content { get; set; }
        [JsonProperty("date")]
        public int DateInt { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// Método para realizar a leitura e organização do contents e da data. Através de expressões regulares consegue obter os dados da imagem
        /// e do contents em sí retirando tags HTML e BBML
        /// 
        /// É feito também neste método o calculo da data, pois o Json retorna uma int com o total de segundos que devem ser adicionados à partir de 01/01/1970
        /// Com isso é montada uma string com a data correta.
        /// </summary>
        public void OrganizarNoticia()
        {
            Image = Regex.Match(Content, @"\[img\](.+?)\[\/img\]", RegexOptions.IgnoreCase).Groups[1].Value;
            if (string.IsNullOrEmpty(Image))
            {
                Image = Regex.Match(Content, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                if (string.IsNullOrEmpty(Image))
                    Image = "logo/notfound.png";
            }
            Content = Regex.Replace(Content, @"\[img\](.+?)\[\/img\]", string.Empty, RegexOptions.IgnoreCase);
            Content = Regex.Replace(Content, @"<[^>]*>|&nbsp;", string.Empty);
            Content = Regex.Replace(Content, @"\[.*?\]", string.Empty);
            Content = Regex.Replace(Content, "&.*?;", string.Empty).Trim();

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(DateInt-25200);
            //AAAA-MM-DD HH:MM
            string format = "yyyy-MM-dd HH:mm:ss";
            //Date = string.Format("{0}-{1}-{2} {3}", dt.Year, dt.Month, dt.Day, dt.TimeOfDay);
            Date = dt.ToString(format);
        }
    }
}
