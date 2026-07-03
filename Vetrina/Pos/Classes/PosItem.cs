using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vetrina.Pos.Classes
{

    public class PosItem
    {

        public string name { get; set; }
        public int qty { get; set; }
        public double price { get; set; }
        public int tax { get; set; }


    }
}