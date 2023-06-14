using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ConsoleApp2
{
    public static class Helpers
    {
        public static Dictionary<Keys, byte> KeyboardMap
        {
            get
            {
                return
                new Dictionary<Keys, byte>{
                    { Keys.D0, 0x0 },
                    { Keys.D1, 0x1 },
                    { Keys.D2, 0x2 },
                    { Keys.D3, 0x3 },
                    { Keys.D4, 0x4 },
                    { Keys.D5, 0x5 },
                    { Keys.D6, 0x6 },
                    { Keys.D7, 0x7 },
                    { Keys.D8, 0x8 },
                    { Keys.D9, 0x9 },
                    { Keys.A, 0xA },
                    { Keys.B, 0xB },
                    { Keys.C, 0xC },
                    { Keys.D, 0xD },
                    { Keys.E, 0xE },
                    { Keys.F, 0xF }
                };
            }
        }
    }
}
