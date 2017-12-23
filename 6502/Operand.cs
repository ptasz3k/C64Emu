using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public enum AddressMode
    {
        Implicit,
        Accumulator,
        Immediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Relative,
        Absolute,
        AbsoluteX,
        AbsoluteY,
        Indirect,
        IndexedIndirect,
        IndirectIndexed,
    }

    public enum Mnemonic
    {
        ADC,
        AND,
        ASL,
        BCC,
        BCS,
        BEQ,
        BIT,
        BMI,
        BNE,
        BPL,
        BRK,
        BVC,
        BVS,
        CLC,
        CLD,
        CLI,
        CLV,
        CMP,
        CPX,
        CPY,
        DEC,
        DEX,
        DEY,
        EOR,
        INC,
        INX,
        INY,
        JMP,
        JSR,
        LDA,
        LDX,
        LDY,
        LSR,
        NOP,
        ORA,
        PHA,
        PHP,
        PLA,
        PLP,
        ROL,
        ROR,
        RTI,
        RTS,
        SBC,
        SEC,
        SED,
        SEI,
        STA,
        STX,
        STY,
        TAX,
        TAY,
        TSX,
        TXA,
        TXS,
        TYA
    }

    public static class Instructions
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void ADC(Operand op, Cpu cpu)
        {
            ;
        }
    }

    public static class OpLogic
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static Operand NextCycleOperand(Operand op, bool pageBoundCrossed = false, byte? addressLo = null, byte? addressHi = null, byte? value = null)
        {
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                (byte)(pageBoundCrossed ? op.MaxTim + 1 : op.MaxTim),
                op.Len,
                op.Run,
                op.RunSpecific,
                (byte)(op.Tim + 1),
                addressLo ?? op.AddressLo,
                addressHi ?? op.AddressHi,
                value ?? op.Value
            );
        }

        public static Operand Read(Operand op, Cpu cpu)
        {
            var pageCross = false;
            byte? addressLo = null;
            byte? addressHi = null;
            byte? value = null;

            switch (op.Tim)
            {
                case 1:
                    // fetch opcode
                    if (cpu.Memory.Mem[cpu.PC] != op.Code)
                    {
                        _logger.Fatal($"Opcode {op.Code} in operand {op.Mnemonic} ({op.AddressMode}) differs from that read from memory ({cpu.Memory.Mem[cpu.PC]}).");
                        throw new InvalidOperationException();
                    }
                    cpu.PC++;
                    break;
                case 2
                when op.AddressMode == AddressMode.Immediate:
                    value = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 2
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.ZeroPage
                || op.AddressMode == AddressMode.ZeroPageX
                || op.AddressMode == AddressMode.ZeroPageY
                || op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY
                || op.AddressMode == AddressMode.IndexedIndirect
                || op.AddressMode == AddressMode.IndirectIndexed:
                    addressLo = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 3
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    addressHi = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPage:
                    value = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPageX:
                    addressLo = (byte)(op.AddressLo + cpu.X);
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPageY:
                    addressLo = (byte)(op.AddressLo + cpu.Y);
                    break;
                case 3
                when op.AddressMode == AddressMode.IndexedIndirect:
                    addressLo = (byte)(op.AddressLo + cpu.X);
                    break;
                case 3
                when op.AddressMode == AddressMode.IndirectIndexed:
                    // we've got to overwrite pointer address here, so we move it temporarily to addressHi
                    addressHi = op.AddressLo;
                    addressLo = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 4
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.ZeroPageX
                || op.AddressMode == AddressMode.ZeroPageY:
                    value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                case 4
                when op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    var offset = op.AddressMode == AddressMode.AbsoluteX ? cpu.X : cpu.Y;
                    if (op.AddressLo + offset > 0xff)
                    {
                        // page cross, read in next cycle
                        addressLo = (byte)(op.AddressLo + offset);
                        addressHi = (byte)(op.AddressHi + 1);
                        pageCross = true;
                    }
                    else
                    {
                        addressLo = (byte)(op.AddressLo + offset);
                        value = cpu.Memory.Mem[addressLo.Value | (op.AddressHi << 8)];
                    }
                    break;
                case 4
                when op.AddressMode == AddressMode.IndexedIndirect:
                    // we've got to overwrite pointer address here, so we move it temporarily to addressHi
                    addressHi = op.AddressLo;
                    addressLo = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 4
                when op.AddressMode == AddressMode.IndirectIndexed:
                    // pointer address is now in addressHi
                    byte pointerHi = (byte)(op.AddressHi + 1);
                    addressHi = cpu.Memory.Mem[pointerHi];
                    break;
                case 5
                when op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                case 5
                when op.AddressMode == AddressMode.IndexedIndirect:
                    // pointer address is now in addressHi
                    pointerHi = (byte)(op.AddressHi + 1);
                    addressHi = cpu.Memory.Mem[pointerHi];
                    break;
                case 5
                when op.AddressMode == AddressMode.IndirectIndexed:
                    if (op.AddressLo + cpu.Y > 0xff)
                    {
                        addressLo = (byte)(op.AddressLo + cpu.Y);
                        addressHi = (byte)(op.AddressHi + 1);
                        pageCross = true;
                    }
                    else
                    {
                        addressLo = (byte)(op.AddressLo + cpu.Y);
                        value = cpu.Memory.Mem[addressLo.Value | (op.AddressHi << 8)];
                    }
                    break;
                case 6
                when op.AddressMode == AddressMode.IndexedIndirect
                || op.AddressMode == AddressMode.IndirectIndexed:
                    value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                default:
                    _logger.Fatal($"");
                    throw new NotImplementedException($"Unknown state for {op.Mnemonic} ({op.AddressMode}), cycle={op.Tim}/{op.MaxTim}.");
                    break;
            }

            var next = NextCycleOperand(op, pageCross, addressLo, addressHi, value);

            if (next.Tim > next.MaxTim)
            {
                next.RunSpecific(next, cpu);
            }

            return next;
        }
    }

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

        public byte AddressLo { get; }

        public byte AddressHi { get; }

        public byte Value { get; }
    }

    public static class Operands
    {
        public static Operand[] List { get; } = new Operand[]
        {
            // ADC
            new Operand(0x69, Mnemonic.ADC, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x65, Mnemonic.ADC, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x75, Mnemonic.ADC, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x6d, Mnemonic.ADC, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x7d, Mnemonic.ADC, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x79, Mnemonic.ADC, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x61, Mnemonic.ADC, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x71, Mnemonic.ADC, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.ADC)

        };
    }
}
