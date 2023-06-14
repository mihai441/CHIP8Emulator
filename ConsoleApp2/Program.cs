
using ConsoleApp2.Backends;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text;

var gameWindowSettings = new GameWindowSettings
{
    UpdateFrequency = 600
};

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2i(1024, 512),
    Profile = ContextProfile.Compatability,
    Title = "Chip8"
};
using (Display display = new Display(gameWindowSettings, nativeWindowSettings)) {
    display.Run();
}


