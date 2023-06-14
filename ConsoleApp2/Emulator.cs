using ConsoleApp2.Backends;
using ConsoleApp2.Models;
using OpenTK.Input;

namespace ConsoleApp2
{
    public class Emulator
    {
        private CPU cpu;
        private Memory memory;
        public bool isRunning = false;
        public Display display;
        private uint Counter;
        private State State;

        public Dictionary<byte, bool> KeysPressedStatus { get; set; }

        public Emulator(byte[] rom, Display _display) {
            display = _display;
            isRunning = true;
            Counter = 0;
            State = new State();
            cpu = new CPU(rom, this, State);
            KeysPressedStatus = new Dictionary<byte, bool>();
        }

        public void KeyDown(byte key)
        {
            if (!KeysPressedStatus.ContainsKey(key))
                KeysPressedStatus.Add(key,true);
        }

        public void KeyUp(byte key)
        {
            if (KeysPressedStatus.ContainsKey(key))
                KeysPressedStatus.Remove(key);
        }

        public void EmulateCycle()
        {
            
            cpu.RunNextInstruction();
            while((Counter % 10 != 0))
            {
                UpdateTimers();
                Counter++;
            }
            Counter++;
        }

        private void UpdateTimers()
        {
            if (State.Delay > 0) State.Delay--;
            if (State.Sound > 0)
            {
                display?.Beep();
                State.Sound--;
            }
        }

        public void Render(byte[] buffer)
        {
            display.Render(buffer);
        }
    }
}
