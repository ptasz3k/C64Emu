using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace C64Emu._6502
{
    public static class Instructions
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void ADC(Operand op, Cpu cpu)
        {
            throw new NotImplementedException();
        }

        public static void LDA(Operand op, Cpu cpu)
        {
            cpu.A = op.Value;
            cpu.P = cpu.P
                .SetOrClear(ProcessorStatus.Z, op.Value == 0)
                .SetOrClear(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void LDX(Operand op, Cpu cpu)
        {
            cpu.X = op.Value;
            cpu.P = cpu.P
                .SetOrClear(ProcessorStatus.Z, op.Value == 0)
                .SetOrClear(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void LDY(Operand op, Cpu cpu)
        {
            cpu.Y = op.Value;
            cpu.P = cpu.P
                .SetOrClear(ProcessorStatus.Z, op.Value == 0)
                .SetOrClear(ProcessorStatus.N, (op.Value & 0x80) != 0);
        }

        public static void EOR(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A ^ op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClear(ProcessorStatus.Z, result == 0)
                .SetOrClear(ProcessorStatus.N, (result & 0x80) != 0);
        }

        public static void AND(Operand op, Cpu cpu)
        {
            var result = (byte)(cpu.A & op.Value);
            cpu.A = result;
            cpu.P = cpu.P
                .SetOrClear(ProcessorStatus.Z, result == 0)
                .SetOrClear(ProcessorStatus.N, (result & 0x80) != 0);
        }
    }
}
