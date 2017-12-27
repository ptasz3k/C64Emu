using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public static class Instructions
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static (byte value, bool c, bool n, bool z, bool v) BinaryADC(byte memory, byte register, bool carry)
        {
            var sum = register + memory + (carry? 1 : 0);
            var result = (byte)(sum & 0xff);
            var c = sum > 0xff;
            var n = (result & 0x80) != 0;
            var z = result == 0;
            var v = ((register ^ result) & (memory ^ result) & 0x80) != 0;

            return (result, c, n, z, v);
        }

        public static Operand ADC(Operand op, Cpu cpu)
        {
            if (cpu.P.IsClr(ProcessorStatus.D))
            {
                (var a, var c, var n, var z, var v) = BinaryADC(op.Value, cpu.A, cpu.P.IsSet(ProcessorStatus.C));
                cpu.A = a;
                cpu.P = cpu.P
                    .SetOrClrIf(ProcessorStatus.C, c)
                    .SetOrClrIf(ProcessorStatus.N, n)
                    .SetOrClrIf(ProcessorStatus.Z, z)
                    .SetOrClrIf(ProcessorStatus.V, v);
            }
            else
            {
                // TODO: implement decimal mode
                throw new NotImplementedException();
            }

            return op;
        }

        public static Operand SBC(Operand op, Cpu cpu)
        {
            if (cpu.P.IsClr(ProcessorStatus.D))
            {
                (var a, var c, var n, var z, var v) = BinaryADC((byte)(op.Value ^ 0xff), cpu.A, cpu.P.IsSet(ProcessorStatus.C));
                cpu.A = a;
                cpu.P = cpu.P
                    .SetOrClrIf(ProcessorStatus.C, c)
                    .SetOrClrIf(ProcessorStatus.N, n)
                    .SetOrClrIf(ProcessorStatus.Z, z)
                    .SetOrClrIf(ProcessorStatus.V, v);
            }
            else
            {
                // TODO: implement decimal mode
                throw new NotImplementedException();
            }

            return op;
        }

        public static Operand CMP(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.A, true);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
            return op;
        }

        public static Operand CPX(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.X, true);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
            return op;
        }

        public static Operand CPY(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.Y, true);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
            return op;
        }

        public static Operand LDA(Operand op, Cpu cpu)
        {
            cpu.A = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
            return op;
        }

        public static Operand LDX(Operand op, Cpu cpu)
        {
            cpu.X = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
            return op;
        }

        public static Operand LDY(Operand op, Cpu cpu)
        {
            cpu.Y = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
            return op;
        }

        public static Operand EOR(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A ^ op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return op;
        }

        public static Operand AND(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A & op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return op;
        }

        public static Operand ORA(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A | op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return op;
        }

        public static Operand BIT(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0)
                .SetOrClrIf(ProcessorStatus.V, (op.Value & 0x40) != 0)
                .SetOrClrIf(ProcessorStatus.Z, (cpu.A & op.Value) == 0);
            return op;
        }

        public static Operand NOP(Operand op, Cpu cpu)
        {
            return op;
        }

        public static Operand CLC(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.C);
            return op;
        }

        public static Operand CLD(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.D);
            return op;
        }

        public static Operand CLI(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.I);
            return op;
        }

        public static Operand CLV(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.V);
            return op;
        }

        public static Operand SEC(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.C);
            return op;
        }

        public static Operand SED(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.D);
            return op;
        }

        public static Operand SEI(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.I);
            return op;
        }
        
        public static Operand INX(Operand op, Cpu cpu)
        {
            cpu.X += 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
            return op;
        }

        public static Operand INY(Operand op, Cpu cpu)
        {
            cpu.Y += 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
            return op;
        }

        public static Operand DEX(Operand op, Cpu cpu)
        {
            cpu.X -= 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
            return op;
        }

        public static Operand DEY(Operand op, Cpu cpu)
        {
            cpu.Y -= 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
            return op;
        }

        public static Operand TAX(Operand op, Cpu cpu)
        {
            cpu.X = cpu.A;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
            return op;
        }

        public static Operand TAY(Operand op, Cpu cpu)
        {
            cpu.Y = cpu.A;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
            return op;
        }

        public static Operand TSX(Operand op, Cpu cpu)
        {
            cpu.X = cpu.S;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
            return op;
        }

        public static Operand TXA(Operand op, Cpu cpu)
        {
            cpu.A = cpu.X;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.A == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.A & 0x80) != 0);
            return op;
        }

        public static Operand TXS(Operand op, Cpu cpu)
        {
            cpu.S = cpu.X;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.S == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.S & 0x80) != 0);
            return op;
        }

        public static Operand TYA(Operand op, Cpu cpu)
        {
            cpu.A = cpu.Y;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.A == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.A & 0x80) != 0);
            return op;
        }

        public static Operand ASL(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value << 1);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, (op.Value & 0x80) != 0)
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand LSR(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value >> 1);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, (op.Value & 0x01) != 0)
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .Clr(ProcessorStatus.N);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand ROL(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value << 1);
            result |= (cpu.P.IsSet(ProcessorStatus.C) ? (byte)0x01 : (byte)0);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, (op.Value & 0x80) != 0)
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand ROR(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value >> 1);
            result |= (cpu.P.IsSet(ProcessorStatus.C) ? (byte)0x80 : (byte)0);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, (op.Value & 0x01) != 0)
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand INC(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value + 1);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand DEC(Operand op, Cpu cpu)
        {
            byte result = (byte)(op.Value - 1);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
            return new Operand(
                op.Code,
                op.Mnemonic,
                op.AddressMode,
                op.MaxTim,
                op.Len,
                op.Run,
                op.RunSpecific,
                op.Tim,
                op.AddressLo,
                op.AddressHi,
                op.Value,
                result
            );
        }

        public static Operand STA(Operand op, Cpu cpu)
        {
            cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = cpu.A;
            return op;
        }

        public static Operand STX(Operand op, Cpu cpu)
        {
            cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = cpu.X;
            return op;
        }

        public static Operand STY(Operand op, Cpu cpu)
        {
            cpu.Memory.Mem[op.AddressLo | (op.AddressHi << 8)] = cpu.Y;
            return op;
        }
    }
}
