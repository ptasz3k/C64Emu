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
            if (op.Value == 0)
            {
                cpu.P |= ProcessorStatus.Z;
            }
            else
            {
                cpu.P &= ~ProcessorStatus.Z;
            }

            if ((op.Value & 0x80) != 0)
            {
                cpu.P |= ProcessorStatus.N;
            }
            else
            {
                cpu.P &= ~ProcessorStatus.N;
            }
        }
    }
}
