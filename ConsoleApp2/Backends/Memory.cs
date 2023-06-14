using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Backends
{
    public class Memory
    {
        public const int StartAdress = 0x200;
        public byte[] RomBytes { get; set; }

        public Memory() {
            RomBytes = new byte[4096];
        }

        public void LoadRom(byte[] romBytes)
        {
            romBytes.CopyTo(RomBytes, StartAdress);
        }



    }
}
