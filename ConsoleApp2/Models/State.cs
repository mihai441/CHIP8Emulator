using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    public class State
    {

        public State()
        {
            V = new byte[16];
        }
        public byte[] V { get; set; }
        public uint I { get; set; }
        public byte Delay { get; set; }
        public byte Sound { get; set; }

    }
}
