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

        }.ToImmutableDictionary(op => op.Code);
    }
}
