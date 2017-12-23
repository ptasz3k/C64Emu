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
            var p = op.Value == 0 ? cpu.P.Set(ProcessorStatus.Z) : cpu.P.Clear(ProcessorStatus.Z);
            cpu.P = (op.Value & 0x80) != 0 ? p.Set(ProcessorStatus.N) : p.Clear(ProcessorStatus.N);
        }
    }
}
