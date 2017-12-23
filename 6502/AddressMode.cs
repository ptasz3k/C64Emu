using System;
using System.Collections.Generic;
using System.Text;

namespace C64Emu._6502
{
    public enum AddressMode
    {
        Implied,
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
}
