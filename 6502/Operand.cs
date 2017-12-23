using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public class Operand
    {
        public Operand(byte code, Mnemonic mnemonic, AddressMode addressMode, byte maxTim, byte len, Func<Operand, Cpu, Operand> run,
            Action<Operand, Cpu> runSpecific, byte tim = 1, byte addressLo = 0, byte addressHi = 0, byte value = 0)
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
        public Action<Operand, Cpu> RunSpecific { get; }

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
    }
}
