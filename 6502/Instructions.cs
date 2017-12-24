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

        public static void ADC(Operand op, Cpu cpu)
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
        }

        public static void SBC(Operand op, Cpu cpu)
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
        }

        public static void CMP(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.A, false);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
        }

        public static void CPX(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.X, false);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
        }

        public static void CPY(Operand op, Cpu cpu)
        {
            (_, var c, var n, var z, _) = BinaryADC((byte)(op.Value ^ 0xff), cpu.Y, false);
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.C, c)
                .SetOrClrIf(ProcessorStatus.N, n)
                .SetOrClrIf(ProcessorStatus.Z, z);
        }

        public static void LDA(Operand op, Cpu cpu)
        {
            cpu.A = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void LDX(Operand op, Cpu cpu)
        {
            cpu.X = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void LDY(Operand op, Cpu cpu)
        {
            cpu.Y = op.Value;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, op.Value == 0)
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void EOR(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A ^ op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
        }

        public static void AND(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A & op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
        }

        public static void ORA(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A | op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, result == 0)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0);
        }

        public static void BIT(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.N, (op.Value & 0x80) != 0)
                .SetOrClrIf(ProcessorStatus.V, (op.Value & 0x40) != 0)
                .SetOrClrIf(ProcessorStatus.Z, (cpu.A & op.Value) == 0);
        }

        public static void NOP(Operand op, Cpu cpu) { }

        public static void CLC(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.C);
        }

        public static void CLD(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.D);
        }

        public static void CLI(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.I);
        }

        public static void CLV(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Clr(ProcessorStatus.V);
        }

        public static void SEC(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.C);
        }

        public static void SED(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.D);
        }

        public static void SEI(Operand op, Cpu cpu)
        {
            cpu.P = cpu.P.Set(ProcessorStatus.I);
        }
        
        public static void INX(Operand op, Cpu cpu)
        {
            cpu.X += 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
        }

        public static void INY(Operand op, Cpu cpu)
        {
            cpu.Y += 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
        }

        public static void DEX(Operand op, Cpu cpu)
        {
            cpu.X -= 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
        }

        public static void DEY(Operand op, Cpu cpu)
        {
            cpu.Y -= 1;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
        }

        public static void TAX(Operand op, Cpu cpu)
        {
            cpu.X = cpu.A;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
        }

        public static void TAY(Operand op, Cpu cpu)
        {
            cpu.Y = cpu.A;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.Y == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.Y & 0x80) != 0);
        }

        public static void TSX(Operand op, Cpu cpu)
        {
            cpu.X = cpu.S;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.X == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.X & 0x80) != 0);
        }

        public static void TXA(Operand op, Cpu cpu)
        {
            cpu.A = cpu.X;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.A == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.A & 0x80) != 0);
        }

        public static void TXS(Operand op, Cpu cpu)
        {
            cpu.S = cpu.X;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.S == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.S & 0x80) != 0);
        }

        public static void TYA(Operand op, Cpu cpu)
        {
            cpu.A = cpu.Y;
            cpu.P = cpu.P
                .SetOrClrIf(ProcessorStatus.Z, cpu.A == 0)
                .SetOrClrIf(ProcessorStatus.N, (cpu.A & 0x80) != 0);
        }

    }
}
