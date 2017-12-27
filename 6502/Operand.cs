using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public class Operand
    {
        public Operand(byte code, Mnemonic mnemonic, AddressMode addressMode, byte maxTim, byte len, Func<Operand, Cpu, Operand> run,
            Func<Operand, Cpu, Operand> runSpecific, byte tim = 1, byte addressLo = 0, byte addressHi = 0, byte value = 0,
            byte result = 0)
        {
            Code = code;
            Mnemonic = mnemonic;
            AddressMode = addressMode;
            MaxTim = maxTim;
            Len = len;
            Run = run;
            RunSpecific = runSpecific;
            Tim = tim;
            AddressLo = addressLo;
            AddressHi = addressHi;
            Value = value;
            Result = result;
        }

        /// <summary>
        /// Opcode
        /// </summary>
        public byte Code { get; }

        /// <summary>
        /// Mnemonic of operand
        /// </summary>
        public Mnemonic Mnemonic { get; }

        /// <summary>
        /// Addressing mode
        /// </summary>
        public AddressMode AddressMode { get; }

        /// <summary>
        /// Maximum cycle count of instruction processing
        /// </summary>
        public byte MaxTim { get; }

        /// <summary>
        /// Instruction length in memory (bytes)
        /// </summary>
        public byte Len { get; }

        /// <summary>
        /// Instruction type processing logic
        /// </summary>
        public Func<Operand, Cpu, Operand> Run { get; }

        /// <summary>
        /// Specific instruction logic
        /// </summary>
        public Func<Operand, Cpu, Operand> RunSpecific { get; }

        /// <summary>
        /// Current cycle in instruction processing
        /// </summary>
        public byte Tim { get; }

        /// <summary>
        /// Lower byte of memory address on which operand operates
        /// </summary>
        public byte AddressLo { get; }

        /// <summary>
        /// Higher byte of memory address on which operand operates
        /// </summary>
        public byte AddressHi { get; }

        /// <summary>
        /// Value on which operand operates
        /// </summary>
        public byte Value { get; }

        /// <summary>
        /// Operand result
        /// </summary>
        public byte Result { get; }

        public override string ToString()
        {
            var str = $"{Mnemonic}";

            switch (AddressMode)
            {
                case AddressMode.Accumulator:
                    str += " A";
                    break;
                case AddressMode.Absolute:
                    str += string.Format(" $");
                    break;
                case AddressMode.AbsoluteX:
                    str += string.Format(" $,X");
                    break;
                case AddressMode.AbsoluteY:
                    str += string.Format(" $,Y");
                    break;
                case AddressMode.Immediate:
                    str += string.Format(" #$");
                    break;
                case AddressMode.Implied:
                    break;
                case AddressMode.IndexedIndirect:
                    str += string.Format(" ($,X)");
                    break;
                case AddressMode.Indirect:
                    str += string.Format(" ($)");
                    break;
                case AddressMode.IndirectIndexed:
                    str += string.Format(" ($),Y");
                    break;
                case AddressMode.Relative:
                    str += string.Format(" #$");
                    break;
                case AddressMode.ZeroPage:
                    str += string.Format(" $");
                    break;
                case AddressMode.ZeroPageX:
                    str += string.Format(" $,X");
                    break;
                case AddressMode.ZeroPageY:
                    str += string.Format(" $, Y");
                    break;
                default:
                    break;
            }

            return str;
        }
    }
}
