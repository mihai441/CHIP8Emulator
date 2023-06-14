using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;
namespace ConsoleApp2.Backends
{
    public class Display : GameWindow
    {
        private Emulator emulator;
        public Display(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            FileDrop += Window_FileDrop;
        }

        private void Window_FileDrop(FileDropEventArgs obj)
        {
            string rom = obj.FileNames[0];
            var romBytes = File.ReadAllBytes(rom);

            emulator = new Emulator(romBytes, this);
        }

        protected override void OnLoad() 
        {
            base.OnLoad();
            GL.ClearColor(Color.Black);
            GL.Color3(Color.White);
            GL.Ortho(0, 64, 32, 0, -1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            SwapBuffers();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (Helpers.KeyboardMap.TryGetValue(e.Key, out byte value))
            {
                emulator.KeyUp(value);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (Helpers.KeyboardMap.TryGetValue(e.Key, out byte value))
            {
                emulator?.KeyDown(value);
                return;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (emulator != null && emulator.isRunning)
            {
                emulator.EmulateCycle();
            }
        }

        public void Beep()
        {
            Task.Run(() =>
            {
                Console.Beep(400, 50);
            });
        }

        public void Render(byte[] buffer)
        {
            if (buffer != null)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        if (buffer[y * 64 + x] > 0)
                        {
                            GL.Rect(x, y, x + 1, y + 1);
                        }
                    }
                }
                SwapBuffers();
            }
        }
    }

}
