using ConsoleApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Backends
{
    public class CPU
    {
        public uint PC { get; set; }
        public Memory Memory { get; set; }
        public State State { get; set; }
        public Stack<uint> Stack { get; set; }
        public byte[] Gfx { get; set; }
        public Emulator Emulator { get; set; }

        private byte[] Fonts =
        {
              0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
              0x20, 0x60, 0x20, 0x20, 0x70, // 1
              0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
              0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
              0x90, 0x90, 0xF0, 0x10, 0x10, // 4
              0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
              0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
              0xF0, 0x10, 0x20, 0x40, 0x40, // 7
              0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
              0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
              0xF0, 0x90, 0xF0, 0x90, 0x90, // A
              0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
              0xF0, 0x80, 0x80, 0x80, 0xF0, // C
              0xE0, 0x90, 0x90, 0x90, 0xE0, // D
              0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
              0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        public CPU(byte[] rom, Emulator emu, State state)
        {
            PC = 0x200;
            Memory = new Memory();
            Memory.LoadRom(rom);
            Fonts.CopyTo(Memory.RomBytes, 0x0);
            State = state;
            Stack = new Stack<uint>();
            Gfx = new byte[64 * 32];
            Emulator = emu;
        }

        public void RunNextInstruction()
        {
            HandleOperation();
            Console.WriteLine();
        }

        private void HandleOperation()
        {

            uint firstByte = Memory.RomBytes[PC];
            var nibble = firstByte >> 4; // First 4 bits contains the opcode value
            var secondByte = Memory.RomBytes[PC + 1];
            string stringTabs = "       ";

            var OpCode = (ushort)(Memory.RomBytes[PC] << 8 | Memory.RomBytes[PC + 1]);

            byte X = (byte)((OpCode & 0x0f00) >> 8);
            byte Y = (byte)((OpCode & 0x00f0) >> 4);
            byte N = (byte)(OpCode & 0x000f);
            byte NN = (byte)(OpCode & 0x00ff);

            ushort NNN = (ushort)(OpCode & 0x0fff);

            Console.Write($"{PC.ToString("X")} - OPCODE: {OpCode.ToString("X")}");
            PC += 2;


            switch (nibble)
            {
                case 0x00:
                    {
                        switch (secondByte)
                        {
                            case 0xe0:
                                {
                                    Console.Write($"{stringTabs}Clear Screen");
                                    Handle00E0();
                                }
                                break;
                            case 0xee:
                                {
                                    Console.Write($"{stringTabs}Return");
                                    Handle00EE();

                                }
                                break;
                            default:
                                {
                                    Console.Write($"{stringTabs}Unknown 0 OPCode");
                                }
                                break;
                        }
                    }
                    break;
                case 0x01:
                    {
                        Handle1NNN(NNN);
                    }
                    break;
                case 0x02:
                    {
                        Handle2NNN(NNN);
                    }
                    break;
                case 0x03:
                    {
                        Handle3XNN(X, NN);
                    }
                    break;
                case 0x04:
                    {
                        Handle4XNN(X, NN);
                    }
                    break;
                case 0x05:
                    {
                        Handle5XY0(X, Y);

                    }
                    break;
                case 0x06:
                    {
                        Handle6XNN(X, NN);
                    }
                    break;
                case 0x07:
                    {
                        Handle7XNN(X, NN);
                    }
                    break;
                case 0x08:
                    {
                        uint secondByteTail = (uint)secondByte & 0x0f;

                        switch (secondByteTail)
                        {
                            case 0x00:
                                {
                                    Handle8XY0(X, Y);
                                    break;
                                }
                            case 0x01:
                                {
                                    Handle8XY1(X, Y);
                                    break;

                                }
                            case 0x02:
                                {
                                    Handle8XY2(X, Y);
                                    break;

                                }
                            case 0x03:
                                {
                                    Handle8XY3(X, Y);
                                    break;

                                }
                            case 0x04:
                                {
                                    Handle8XY4(X, Y);
                                    break;

                                }
                            case 0x05:
                                {
                                    Handle8XY5(X, Y);
                                    break;

                                }
                            case 0x06:
                                {
                                    Handle8XY6(X, Y);
                                    break;

                                }
                            case 0x07:
                                {
                                    Handle8XY7(X, Y);
                                    break;

                                }
                            case 0x0E:
                                {
                                    Handle8XYE(X, Y);
                                    break;
                                }
                            default:
                                Console.WriteLine($"OPCode with 2nd byte {secondByte} not found!");
                                break;
                        }
                    }
                    break;
                case 0x09:
                    {
                        Handle9XY0(X, Y);
                    }
                    break;
                case 0x0a:
                    {
                        HandleANNN(NNN);
                    }
                    break;
                case 0x0b:
                    {
                        HandleBNNN(NNN);
                    }
                    break;
                case 0x0c:
                    {
                        HandleCXNN(X, NN);
                    }
                    break;
                case 0x0D:
                    {
                        HandleDXYN(X, Y, N);
                    }
                    break;
                case 0x0E:
                    {
                        uint secondByteTail = (uint)secondByte & 0x0f;

                        switch (secondByteTail)
                        {
                            case 0x0E:
                                {
                                    HandleEX9E(X, NN);
                                }
                                break;
                            case 0x01:
                                {
                                    HandleEXA1(X);

                                }
                                break;
                            default:
                                Console.WriteLine($"OPCode with 2nd byte {secondByte} not found!");
                                break;
                        }
                        break;
                    }
                case 0x0F:
                    {
                        switch (secondByte)
                        {
                            case 0x07:
                                HandleFX07(X);
                                break;
                            case 0x0A:
                                HandleFX0A(X);
                                break;
                            case 0x15:
                                HandleFX15(X);
                                break;
                            case 0x18:
                                HandleFX18(X);
                                break;
                            case 0x1E:
                                HandleFX1E(X);
                                break;
                            case 0x29:
                                HandleFX29(X);
                                break;
                            case 0x33:
                                HandleFX33(X);
                                break;
                            case 0x55:
                                HandleFX55(X);
                                break;
                            case 0x65:
                                HandleFX65(X);
                                break;
                            default:
                                Console.WriteLine($"OPCode with 2nd byte {secondByte} not found!");
                                break;
                        }
                        break;
                    }
                    default:
                    Console.WriteLine($"No OPCOde with value {OpCode} found!");
                    break;
            }
        }

        private void Handle00E0()
        {
            Gfx = new byte[64 * 32];
        }

        private void Handle00EE() // Returns from a subroutine.
        {
            PC = Stack.Pop();
        }
        private void Handle1NNN(uint NNN)   //Jumps to address NNN.
        {
            PC = NNN;
        }

        private void Handle2NNN(uint NNN) //Calls subroutine at NNN. GOTO
        {
            Stack.Push(PC);
            PC = NNN;
        }

        private void Handle3XNN(byte X, byte NN) //Skips the next instruction if VX equals NN 
        {
            if (State.V[X] == NN)
                PC += 2;
        }

        private void Handle4XNN(byte X, byte NN) //Skips the next instruction if VX does not equal NN
        {
            if (State.V[X] != NN)
                PC += 2;
        }

        private void Handle5XY0(byte X, byte Y) // Skips the next instruction if VX equals VY 
        {
            if (State.V[X] == State.V[Y])
                PC += 2;
        }

        private void Handle6XNN(byte X, byte NN) //Sets VX to NN
        {
            State.V[X] = NN;
        }

        private void Handle7XNN(byte X, byte NN) //Adds NN to VX (carry flag is not changed).
        {
            State.V[X] += (byte)NN;
        }

        private void Handle8XY0(byte X, byte Y) //Sets VX to the value of VY.
        {
            State.V[X] = State.V[Y];
        }

        private void Handle8XY1(byte X, byte Y) //Sets VX to VX or VY. (bitwise OR operation)

        {
            State.V[X] |= State.V[Y];
        }

        private void Handle8XY2(byte X, byte Y) //Sets VX to VX and VY. (bitwise AND operation)

        {
            State.V[X] &= State.V[Y];
        }

        private void Handle8XY3(byte X, byte Y) //Sets VX to VX xor VY.
        {
            State.V[X] ^= State.V[Y];
        }


        private void Handle8XY4(byte X, byte Y) //Adds VY to VX.VF is set to 1 when there's a carry, and to 0 when there is not.
        {
            var result = State.V[X] + State.V[Y];
            bool setVF1 = ((result) & 0xff00) > 0 ? true : false;

            State.V[X] = (byte)(result & 0xff);
            State.V[0xF] = setVF1 ? (byte)1 : (byte)0;
        }

        private void Handle8XY5(byte X, byte Y) //VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there is not.
        {
            bool setVF1 = State.V[X] >= State.V[Y] ? true : false;


            State.V[X] = (byte)(((uint)State.V[X] - (uint)State.V[Y]) % 0xff);
            State.V[0xF] = setVF1 ? (byte)1 : (byte)0;
        }

        private void Handle8XY6(byte X, byte Y) //Stores the least significant bit of VX in VF and then shifts VX to the right by 1.
        {
            byte leastSignificantBit = (byte)(State.V[X] & 0x1);

            State.V[X] >>= 1;
            State.V[0xF] = leastSignificantBit;
        }

        private void Handle8XY7(byte X, byte Y) //Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there is not.
        {

            bool setVF1 = State.V[Y] < State.V[X] ? false : true;

            State.V[X] = (byte)((State.V[Y] - State.V[X]) % 255);
            State.V[0xF] = setVF1 ? (byte)1 : (byte)0;
        }


        private void Handle8XYE(byte X, byte Y) //Stores the most significant bit of VX in VF and then shifts VX to the left by 1.
        {
            byte mostSignificantBit = (byte)((byte)(State.V[X] & 0x80) >> 7);
            
            State.V[X] <<= 1;

            State.V[0xF] = mostSignificantBit;
        }

        private void Handle9XY0(byte X, byte Y) //Skips the next instruction if VX does not equal VY. (Usually the next instruction is a jump to skip a code block)
        {

            if (State.V[X] != State.V[Y])
            {
                PC += 2;
            }
        }

        private void HandleANNN(ushort NNN) //Sets I to the address NNN.
        {
            State.I = NNN;
        }

        private void HandleBNNN(uint NNN) //Jumps to the address NNN plus V0.
        {
            PC = State.V[0] + NNN;
        }

        private void HandleCXNN(byte X, byte NN) //Sets VX to the result of a bitwise and operation on a random number and NN
        {
            Random rnd = new Random();
            byte randomNumber = (byte)rnd.Next(0xFF);
            State.V[X] = (byte)(randomNumber & NN);
        }

        private void HandleDXYN(byte X, byte Y, byte N) // Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. Each row of 8 pixels is read as bit-coded starting from memory location I; I value does not change after the execution of this instruction. As described above, VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, and to 0 if that does not happen.
        {
            State.V[0xF] = 0;

            // Draw N lines on the screen.
            for (int line = 0; line < N; line++)
            {
                // y is the starting line Y + current line. If y is larger than the total width of the screen then wrap around (this is the modulo operation).
                var y = (State.V[Y] + line) % 32;

                // The current sprite being drawn, each line is a new sprite.
                byte sprite = Memory.RomBytes[State.I + line];

                // Each bit in the sprite is a pixel on or off.
                for (int column = 0; column < 8; column++)
                {
                    // Start with the current most significant bit. The next bit will be left shifted in from the right.
                    if ((sprite & 0x80) != 0)
                    {
                        // Get the current x position and wrap around if needed.
                        var x = (State.V[X] + column) % 64;

                        // Collision detection: If the target pixel is already set then set the collision detection flag in register VF.
                        if (Gfx[y * 64 + x] == 1)
                        {
                            State.V[0xF] = 1;
                        }

                        // Enable or disable the pixel (XOR operation).
                        Gfx[y * 64 + x] ^= 1;
                    }

                    // Shift the next bit in from the right.
                    sprite <<= 0x1;
                }
            }

            Emulator?.Render(Gfx);
        }

        private void HandleEX9E(byte X, byte NN) //Skips the next instruction if the key stored in VX is pressed (usually the next instruction is a jump to skip a code block).
        {
            if (Emulator.KeysPressedStatus.ContainsKey((byte)State.V[X]))
                PC += 2;
        }

        private void HandleEXA1(byte X) //Skips the next instruction if the key stored in VX is not pressed (usually the next instruction is a jump to skip a code block).
        {
            if (!Emulator.KeysPressedStatus.ContainsKey((byte)State.V[X]))
                PC += 2;
        }

        private void HandleFX07(byte X) //Sets VX to the value of the delay timer.
        {
            State.V[X] = State.Delay;
        }

        private void HandleFX0A(byte X) // A key press is awaited, and then stored in VX(blocking operation, all instruction halted until next key event).
        {
            while (true)
            {
                if (Emulator.KeysPressedStatus.Any())
                {
                    State.V[X] = Emulator.KeysPressedStatus.FirstOrDefault().Key;
                    break;
                }
                Display.ProcessWindowEvents(false);
            }
        }

        private void HandleFX15(byte X) //Sets the delay timer to VX.
        {
            State.Delay = State.V[X];
        }

        private void HandleFX18(byte X) //Sets the sound timer to VX.
        {
            State.Sound = State.V[X];
        }

        private void HandleFX1E(byte X) //Adds VX to I. VF is not affected
        {
            State.I += State.V[X];
        }

        private void HandleFX29(byte X) //Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font.
        {
            State.I = (byte)(State.V[X] * 5);
        }

        private void HandleFX33(byte X) //Stores the binary-coded decimal representation of VX, with the hundreds digit in memory at location in I, the tens digit at location I+1, and the ones digit at location I+2.
        {
            var number = State.V[X];
            Memory.RomBytes[State.I] = (byte) (number / 100);
            Memory.RomBytes[State.I + 1] = (byte)((number / 10) % 10);
            Memory.RomBytes[State.I + 2] = (byte)((number % 100) % 10);
        }

        private void HandleFX55(byte X)
        {
            for(int i=0; i<= X; i++)
            {
                Memory.RomBytes[State.I + i] = (byte)State.V[i];
            }
        }

        private void HandleFX65(byte X)
        {
            for (int i = 0; i <= X; i++)
            {
                State.V[i] = Memory.RomBytes[State.I + i];
            }
        }

    }
}
