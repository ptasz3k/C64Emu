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

        private static Operand IncreaseCycleCount(Operand op, bool pageBoundCrossed = false)
        {
            return new Operand
            {
                Code = op.Code,
                Mnemonic = op.Mnemonic,
                AddressMode = op.AddressMode,
                Tim = (byte)(op.Tim + 1),
                MaxTim = (byte)(pageBoundCrossed ? op.MaxTim + 1 : op.MaxTim),
                Len = op.Len,
                Run = op.Run,
                RunSpecific = op.RunSpecific,
                AddressLo = op.AddressLo,
                AddressHi = op.AddressHi,
                Value = op.Value
            };
        }

        public static Operand Read(Operand op, Cpu cpu)
        {
            var pageCross = false;

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
                    op.Value = cpu.Memory.Mem[cpu.PC];
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
                    op.AddressLo = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 3
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    op.AddressHi = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPage:
                    op.Value = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPageX:
                    op.AddressLo += cpu.X;
                    op.AddressHi = 0; 
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPageY:
                    op.AddressLo += cpu.Y;
                    op.AddressHi = 0; 
                    break;
                case 3
                when op.AddressMode == AddressMode.IndexedIndirect:
                    op.AddressLo += cpu.X;
                    break;
                case 3
                when op.AddressMode == AddressMode.IndirectIndexed:
                    // we've got to overwrite pointer address here, so we move it temporarily to addressHi
                    op.AddressHi = op.AddressLo;
                    op.AddressLo = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 4
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.ZeroPageX
                || op.AddressMode == AddressMode.ZeroPageY:
                    op.Value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                case 4
                when op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    var offset = op.AddressMode == AddressMode.AbsoluteX ? cpu.X : cpu.Y;
                    if (op.AddressLo + offset > 0xff)
                    {
                        // page cross, read in next cycle
                        op.AddressLo += offset;
                        op.AddressHi += 1;
                        pageCross = true;
                    }
                    else
                    {
                        op.AddressLo += offset;
                        op.Value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    }
                    break;
                case 4
                when op.AddressMode == AddressMode.IndexedIndirect:
                    // we've got to overwrite pointer address here, so we move it temporarily to addressHi
                    op.AddressHi = op.AddressLo;
                    op.AddressLo = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 4
                when op.AddressMode == AddressMode.IndirectIndexed:
                    // pointer address is now in addressHi
                    byte pointerHi = (byte)(op.AddressHi + 1);
                    op.AddressHi = cpu.Memory.Mem[pointerHi];
                    break;
                case 5
                when op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.AbsoluteY:
                    op.Value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                case 5
                when op.AddressMode == AddressMode.IndexedIndirect:
                    // pointer address is now in addressHi
                    pointerHi = (byte)(op.AddressHi + 1);
                    op.AddressHi = cpu.Memory.Mem[pointerHi];
                    break;
                case 5
                when op.AddressMode == AddressMode.IndirectIndexed:
                    if (op.AddressLo + cpu.Y > 0xff)
                    {
                        op.AddressLo += cpu.Y;
                        op.AddressHi += 1;
                        pageCross = true;
                    }
                    else
                    {
                        op.AddressLo += cpu.Y;
                        op.Value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    }
                    break;
                case 6
                when op.AddressMode == AddressMode.IndexedIndirect
                || op.AddressMode == AddressMode.IndirectIndexed:
                    op.Value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;


                default:
                    _logger.Fatal($"");
                    throw new NotImplementedException($"Unknown state for {op.Mnemonic} ({op.AddressMode}), cycle={op.Tim}/{op.MaxTim}.");
                    break;
            }

            var next = IncreaseCycleCount(op, pageCross);

            if (next.Tim > next.MaxTim)
            {
                op.RunSpecific(op, cpu);
            }

            return next;
        }
    }

    public class Operand
    {
        /// <summary>
        /// Opcode
        /// </summary>
        public byte Code { get; set; }

        /// <summary>
        /// Mnemonic of operand
        /// </summary>
        public Mnemonic Mnemonic { get; set; }

        /// <summary>
        /// Addressing mode
        /// </summary>
        public AddressMode AddressMode {get;set;}

        /// <summary>
        /// Current cycle in instruction processing
        /// </summary>
        public byte Tim { get; set; } = 1;

        /// <summary>
        /// Maximum cycle count of instruction processing
        /// </summary>
        public byte MaxTim { get; set; }

        /// <summary>
        /// Instruction length in memory (bytes)
        /// </summary>
        public byte Len { get; set; }

        /// <summary>
        /// Instruction type processing logic
        /// </summary>
        public Func<Operand, Cpu, Operand> Run { get; set; }

        /// <summary>
        /// Specific instruction logic
        /// </summary>
        public Action<Operand, Cpu> RunSpecific { get; set; }

        public byte AddressLo { get; set; } = 0;

        public byte AddressHi { get; set; } = 0;

        public byte Value { get; set; } = 0;
    }

    public static class Operands
    {
        public static Operand[] List { get; } = new Operand[]
        {
            // ADC
            new Operand { Code = 0x69, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.Immediate, MaxTim = 2, Len = 2, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x65, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.ZeroPage, MaxTim = 3, Len = 2, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x75, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.ZeroPageX, MaxTim = 4, Len = 2, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x6d, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.Absolute, MaxTim = 4, Len = 3, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x7d, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.AbsoluteX, MaxTim = 4, Len = 3, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x79, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.AbsoluteY, MaxTim = 4, Len = 3, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x61, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.IndexedIndirect, MaxTim = 6, Len = 2, Run = OpLogic.Read, RunSpecific = Instructions.ADC },
            new Operand { Code = 0x71, Mnemonic = Mnemonic.ADC, AddressMode = AddressMode.IndirectIndexed, MaxTim = 5, Len = 2, Run = OpLogic.Read, RunSpecific = Instructions.ADC }

        };
    }
}
