using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoFinal
{
    /// <summary>
    /// Classe criada para agregar as noticias que serão lidas dos feeds (XML)
    /// </summary>
    class GameNew
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string GameID { get; set; }
    }
}
