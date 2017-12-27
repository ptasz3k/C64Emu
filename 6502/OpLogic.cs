using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public static class OpLogic
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static Operand NextCycleOperand(Operand op, bool pageBoundCrossed = false, byte? addressLo = null, byte? addressHi = null,
            byte? value = null, byte? result = null)
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
                value ?? op.Value,
                result ?? op.Result
            );
        }

        public static Operand ReadModifyWrite(Operand op, Cpu cpu)
        {
            const bool pageCross = false;
            byte? addressLo = null;
            byte? addressHi = null;
            byte? value = null;

            switch (op.Tim)
            {
                case 1
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.ZeroPage
                || op.AddressMode == AddressMode.ZeroPageX
                || op.AddressMode == AddressMode.AbsoluteX
                || op.AddressMode == AddressMode.Accumulator:
                    // fetch opcode
                    if (cpu.Memory.Mem[cpu.PC] != op.Code)
                    {
                        _logger.Fatal($"Opcode {op.Code} in operand {op.Mnemonic} ({op.AddressMode}) differs from that read from memory ({cpu.Memory.Mem[cpu.PC]}).");
                        throw new InvalidOperationException();
                    }
                    cpu.PC++;
                    break;
                case 2
                when op.AddressMode == AddressMode.Accumulator:
                    // read value from accumulator, generate next operand immediately, run specific op and move result to accumulator
                    op = NextCycleOperand(op, value: cpu.A);
                    op = op.RunSpecific(op, cpu);
                    cpu.A = op.Result;
                    break;
                case 2
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.ZeroPage
                || op.AddressMode == AddressMode.ZeroPageX
                || op.AddressMode == AddressMode.AbsoluteX:
                    addressLo = cpu.Memory.Mem[cpu.PC];
                    cpu.PC++;
                    break;
                case 3
                when op.AddressMode == AddressMode.Absolute
                || op.AddressMode == AddressMode.AbsoluteX:
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
                case 4
                when op.AddressMode == AddressMode.Absolute:
                    value = cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)];
                    break;
                case 4
                when op.AddressMode == AddressMode.ZeroPage:
                    // write value back to RAM, and then run an operation on it, correct result will be written in next cycle
                    cpu.Memory.Mem[op.AddressLo] = op.Value;
                    op = op.RunSpecific(op, cpu);
                    break;
                case 4
                when op.AddressMode == AddressMode.ZeroPageX:
                    // FIXME: can be linked with Absolute address mode case
                    value = cpu.Memory.Mem[op.AddressLo];
                    break;
                case 4
                when op.AddressMode == AddressMode.AbsoluteX:
                    var effectiveOffset = op.AddressLo + cpu.X;
                    addressLo = (byte)effectiveOffset;
                    addressHi = (byte)(op.AddressHi + effectiveOffset / 0x100);
                    // read from effective address, it may be invalid at this time (smaller by $100)
                    value = cpu.Memory.Mem[addressLo.Value | (op.AddressHi << 8)];
                    break;
                case 5
                when op.AddressMode == AddressMode.Absolute:
                    // write value back to RAM, and then run an operation on it, correct result will be written in next cycle
                    cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = op.Value;
                    op = op.RunSpecific(op, cpu);
                    break;
                case 5
                when op.AddressMode == AddressMode.ZeroPage:
                    cpu.Memory.Mem[op.AddressLo] = op.Result;
                    break;
                case 5
                when op.AddressMode == AddressMode.ZeroPageX:
                    // FIXME: can be linked with Absolute address mode case
                    // write value back to RAM, and then run an operation on it, correct result will be written in next cycle
                    cpu.Memory.Mem[op.AddressLo] = op.Value;
                    op = op.RunSpecific(op, cpu);
                    break;
                case 5
                when op.AddressMode == AddressMode.AbsoluteX:
                    // read from valid address
                    value = cpu.Memory.Mem[op.AddressLo + (op.AddressHi << 8)];
                    break;
                case 6
                when op.AddressMode == AddressMode.Absolute:
                    cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = op.Result;
                    break;
                case 6
                when op.AddressMode == AddressMode.ZeroPageX:
                    // FIXME: can be linked with Absolute address mode case
                    cpu.Memory.Mem[op.AddressLo] = op.Result;
                    break;
                case 6
                when op.AddressMode == AddressMode.AbsoluteX:
                    // write the value back to effective address, and then run an operation on it,
                    // correct result will be written in next cycle
                    cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = op.Value;
                    op = op.RunSpecific(op, cpu);
                    break;
                case 7
                when op.AddressMode == AddressMode.AbsoluteX:
                    cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = op.Result;
                    break;
                default:
                    _logger.Fatal($"Unknown state for {op.Mnemonic} ({op.AddressMode}), cycle={op.Tim}/{op.MaxTim}.");
                    throw new NotImplementedException($"Unknown state for {op.Mnemonic} ({op.AddressMode}), cycle={op.Tim}/{op.MaxTim}.");
                    break;
            }

            var next = (op.Tim <= op.MaxTim) ? NextCycleOperand(op, pageCross, addressLo, addressHi, value) : op;

            return next;
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
                when op.AddressMode == AddressMode.Implied:
                    value = cpu.Memory.Mem[cpu.PC]; // FIXME: just for fun, cpu does it, but it throws this value away
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
                    value = cpu.Memory.Mem[op.AddressLo];
                    addressLo = (byte)(op.AddressLo + cpu.X);
                    break;
                case 3
                when op.AddressMode == AddressMode.ZeroPageY:
                    value = cpu.Memory.Mem[op.AddressLo];
                    addressLo = (byte)(op.AddressLo + cpu.Y);
                    break;
                case 3
                when op.AddressMode == AddressMode.IndexedIndirect:
                    value = cpu.Memory.Mem[op.AddressLo];
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
                        // read from invalid address
                        value = cpu.Memory.Mem[addressLo.Value | (op.AddressHi << 8)];
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
                        // read from invalid address, it's smaller by $100 from valid address
                        value = cpu.Memory.Mem[addressLo.Value | (op.AddressHi << 8)];
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
}
