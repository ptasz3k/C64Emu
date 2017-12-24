using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public static class Instructions
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static void BinaryADC(byte value, Cpu cpu, bool actAsCMP = false)
        {
            var sum = cpu.A + value + (cpu.P.IsSet(ProcessorStatus.C) ? 1 : 0);
            var result = (byte)(sum & 0xff);
            var p = cpu.P
                .SetOrClrIf(ProcessorStatus.C, sum > 0xff)
                .SetOrClrIf(ProcessorStatus.N, (result & 0x80) != 0)
                .SetOrClrIf(ProcessorStatus.Z, result == 0);

            if (!actAsCMP)
            {
                p = p.SetOrClrIf(ProcessorStatus.V, ((cpu.A ^ result) & (value ^ result) & 0x80) != 0);
                cpu.A = result;
            }

            cpu.P = p;
        }

        public static void ADC(Operand op, Cpu cpu)
        {
            if (cpu.P.IsClr(ProcessorStatus.D))
            {
                BinaryADC(op.Value, cpu);
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
                BinaryADC((byte)(op.Value ^ 0xff), cpu);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void CMP(Operand op, Cpu cpu)
        {
            BinaryADC((byte)(op.Value ^ 0xff), cpu, true);
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
    }
}
