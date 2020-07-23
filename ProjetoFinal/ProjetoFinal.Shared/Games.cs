using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal
{
    class Games
    {
        public int Appid{ get; set; }
        public string Name { get; set; }
        public Games(int appid, string name)
        {
            Appid = appid;
            Name = name;
        }
    }
}
