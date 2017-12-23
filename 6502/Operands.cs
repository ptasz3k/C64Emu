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
            new Operand(0x71, Mnemonic.ADC, AddressMode.IndirectIndexed, 5, 2, OpLogic.Read, Instructions.ADC)
        }.ToImmutableDictionary(op => op.Code);
    }
}
