using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace C64Emu._6502
{
    public static class Operands
    {
        public static ImmutableDictionary<byte, Operand> List { get; } = new Operand[]
        {
            // ADC
            new Operand(0x69, Mnemonic.ADC, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x65, Mnemonic.ADC, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x75, Mnemonic.ADC, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x6d, Mnemonic.ADC, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x7d, Mnemonic.ADC, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x79, Mnemonic.ADC, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.ADC),
            new Operand(0x61, Mnemonic.ADC, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.ADC),
            new Operand(0x71, Mnemonic.ADC, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.ADC),

            // LDA
            new Operand(0xa9, Mnemonic.LDA, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.LDA),
            new Operand(0xa5, Mnemonic.LDA, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.LDA),
            new Operand(0xb5, Mnemonic.LDA, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.LDA),
            new Operand(0xad, Mnemonic.LDA, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.LDA),
            new Operand(0xbd, Mnemonic.LDA, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.LDA),
            new Operand(0xb9, Mnemonic.LDA, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.LDA),
            new Operand(0xa1, Mnemonic.LDA, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.LDA),
            new Operand(0xb1, Mnemonic.LDA, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.LDA),

            // LDX
            new Operand(0xa2, Mnemonic.LDX, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.LDX),
            new Operand(0xa6, Mnemonic.LDX, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.LDX),
            new Operand(0xb6, Mnemonic.LDX, AddressMode.ZeroPageY, 4, 2, OpLogic.Read, Instructions.LDX),
            new Operand(0xae, Mnemonic.LDX, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.LDX),
            new Operand(0xbe, Mnemonic.LDX, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.LDX),

            // LDY
            new Operand(0xa0, Mnemonic.LDY, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.LDY),
            new Operand(0xa4, Mnemonic.LDY, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.LDY),
            new Operand(0xb4, Mnemonic.LDY, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.LDY),
            new Operand(0xac, Mnemonic.LDY, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.LDY),
            new Operand(0xbc, Mnemonic.LDY, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.LDY),

            // EOR
            new Operand(0x49, Mnemonic.EOR, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.EOR),
            new Operand(0x45, Mnemonic.EOR, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.EOR),
            new Operand(0x55, Mnemonic.EOR, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.EOR),
            new Operand(0x4d, Mnemonic.EOR, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.EOR),
            new Operand(0x5d, Mnemonic.EOR, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.EOR),
            new Operand(0x59, Mnemonic.EOR, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.EOR),
            new Operand(0x41, Mnemonic.EOR, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.EOR),
            new Operand(0x51, Mnemonic.EOR, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.EOR),

            // AND
            new Operand(0x29, Mnemonic.AND, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.AND),
            new Operand(0x25, Mnemonic.AND, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.AND),
            new Operand(0x35, Mnemonic.AND, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.AND),
            new Operand(0x2d, Mnemonic.AND, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.AND),
            new Operand(0x3d, Mnemonic.AND, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.AND),
            new Operand(0x39, Mnemonic.AND, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.AND),
            new Operand(0x21, Mnemonic.AND, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.AND),
            new Operand(0x31, Mnemonic.AND, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.AND),

            // ORA
            new Operand(0x09, Mnemonic.ORA, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.ORA),
            new Operand(0x05, Mnemonic.ORA, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.ORA),
            new Operand(0x15, Mnemonic.ORA, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.ORA),
            new Operand(0x0d, Mnemonic.ORA, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.ORA),
            new Operand(0x1d, Mnemonic.ORA, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.ORA),
            new Operand(0x19, Mnemonic.ORA, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.ORA),
            new Operand(0x01, Mnemonic.ORA, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.ORA),
            new Operand(0x11, Mnemonic.ORA, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.ORA),

            // SBC
            new Operand(0xe9, Mnemonic.SBC, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.SBC),
            new Operand(0xe5, Mnemonic.SBC, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.SBC),
            new Operand(0xf5, Mnemonic.SBC, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.SBC),
            new Operand(0xed, Mnemonic.SBC, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.SBC),
            new Operand(0xfd, Mnemonic.SBC, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.SBC),
            new Operand(0xf9, Mnemonic.SBC, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.SBC),
            new Operand(0xe1, Mnemonic.SBC, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.SBC),
            new Operand(0xf1, Mnemonic.SBC, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.SBC),

            // CMP
            new Operand(0xc9, Mnemonic.CMP, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.CMP),
            new Operand(0xc5, Mnemonic.CMP, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.CMP),
            new Operand(0xd5, Mnemonic.CMP, AddressMode.ZeroPageX, 4, 2, OpLogic.Read, Instructions.CMP),
            new Operand(0xcd, Mnemonic.CMP, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.CMP),
            new Operand(0xdd, Mnemonic.CMP, AddressMode.AbsoluteX, 4, 3, OpLogic.Read, Instructions.CMP),
            new Operand(0xd9, Mnemonic.CMP, AddressMode.AbsoluteY, 4, 3, OpLogic.Read, Instructions.CMP),
            new Operand(0xc1, Mnemonic.CMP, AddressMode.IndexedIndirect, 6, 2, OpLogic.Read, Instructions.CMP),
            new Operand(0xd1, Mnemonic.CMP, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.CMP),

            /// CPX
            new Operand(0xe0, Mnemonic.CPX, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.CPX),
            new Operand(0xe4, Mnemonic.CPX, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.CPX),
            new Operand(0xec, Mnemonic.CPX, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.CPX),

            /// CPY
            new Operand(0xc0, Mnemonic.CPY, AddressMode.Immediate, 2, 2, OpLogic.Read, Instructions.CPY),
            new Operand(0xc4, Mnemonic.CPY, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.CPY),
            new Operand(0xcc, Mnemonic.CPY, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.CPY),

            // BIT
            new Operand(0x24, Mnemonic.BIT, AddressMode.ZeroPage, 3, 2, OpLogic.Read, Instructions.BIT),
            new Operand(0x2c, Mnemonic.BIT, AddressMode.Absolute, 4, 3, OpLogic.Read, Instructions.BIT),

            // NOP
            new Operand(0xea, Mnemonic.NOP, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.NOP),

            // Clear processor flag instructions
            new Operand(0x18, Mnemonic.CLC, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.CLC),
            new Operand(0xd8, Mnemonic.CLD, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.CLD),
            new Operand(0x58, Mnemonic.CLI, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.CLI),
            new Operand(0xb8, Mnemonic.CLV, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.CLV),

            // Clear processor flag instructions
            new Operand(0x38, Mnemonic.SEC, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.SEC),
            new Operand(0xf8, Mnemonic.SED, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.SED),
            new Operand(0x78, Mnemonic.SEI, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.SEI),

            // INX, INY, DEX, DEY
            new Operand(0xe8, Mnemonic.INX, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.INX),
            new Operand(0xc8, Mnemonic.INY, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.INY),
            new Operand(0xca, Mnemonic.DEX, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.DEX),
            new Operand(0x88, Mnemonic.DEY, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.DEY),

            // Register transfer instructions
            new Operand(0xaa, Mnemonic.TAX, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TAX),
            new Operand(0xa8, Mnemonic.TAY, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TAY),
            new Operand(0xba, Mnemonic.TSX, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TSX),
            new Operand(0x8a, Mnemonic.TXA, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TXA),
            new Operand(0x9a, Mnemonic.TXS, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TXS),
            new Operand(0x98, Mnemonic.TYA, AddressMode.Implied, 2, 1, OpLogic.Read, Instructions.TYA),

            // TODO: LAX

            // Read-Modify-Write instructions
            new Operand(0x0a, Mnemonic.ASL, AddressMode.Accumulator, 2, 1, OpLogic.ReadModifyWrite, Instructions.ASL),
            new Operand(0x06, Mnemonic.ASL, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.ASL),
            new Operand(0x16, Mnemonic.ASL, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.ASL),
            new Operand(0x0e, Mnemonic.ASL, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.ASL),
            new Operand(0x1e, Mnemonic.ASL, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.ASL),

            new Operand(0x4a, Mnemonic.LSR, AddressMode.Accumulator, 2, 1, OpLogic.ReadModifyWrite, Instructions.LSR),
            new Operand(0x46, Mnemonic.LSR, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.LSR),
            new Operand(0x56, Mnemonic.LSR, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.LSR),
            new Operand(0x4e, Mnemonic.LSR, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.LSR),
            new Operand(0x5e, Mnemonic.LSR, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.LSR),

            new Operand(0x2a, Mnemonic.ROL, AddressMode.Accumulator, 2, 1, OpLogic.ReadModifyWrite, Instructions.ROL),
            new Operand(0x26, Mnemonic.ROL, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.ROL),
            new Operand(0x36, Mnemonic.ROL, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.ROL),
            new Operand(0x2e, Mnemonic.ROL, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.ROL),
            new Operand(0x3e, Mnemonic.ROL, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.ROL),

            new Operand(0x6a, Mnemonic.ROR, AddressMode.Accumulator, 2, 1, OpLogic.ReadModifyWrite, Instructions.ROR),
            new Operand(0x66, Mnemonic.ROR, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.ROR),
            new Operand(0x76, Mnemonic.ROR, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.ROR),
            new Operand(0x6e, Mnemonic.ROR, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.ROR),
            new Operand(0x7e, Mnemonic.ROR, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.ROR),

            new Operand(0xe6, Mnemonic.INC, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.INC),
            new Operand(0xf6, Mnemonic.INC, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.INC),
            new Operand(0xee, Mnemonic.INC, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.INC),
            new Operand(0xfe, Mnemonic.INC, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.INC),

            new Operand(0xc6, Mnemonic.DEC, AddressMode.ZeroPage, 5, 2, OpLogic.ReadModifyWrite, Instructions.DEC),
            new Operand(0xd6, Mnemonic.DEC, AddressMode.ZeroPageX, 6, 2, OpLogic.ReadModifyWrite, Instructions.DEC),
            new Operand(0xce, Mnemonic.DEC, AddressMode.Absolute, 6, 3, OpLogic.ReadModifyWrite, Instructions.DEC),
            new Operand(0xde, Mnemonic.DEC, AddressMode.AbsoluteX, 7, 3, OpLogic.ReadModifyWrite, Instructions.DEC),

            // TODO: SLO, SRE, RLA, RRA, ISB, DCP

        }.ToImmutableDictionary(op => op.Code);
    }
}
