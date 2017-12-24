using System;
using System.Collections.Generic;
using System.Text;

namespace C64Emu._6502
{
    [Flags]
    public enum ProcessorStatus
    {
        /// <summary>
        /// Negative flag
        /// </summary>
        N = 0x80,

        /// <summary>
        /// oVerflow flag
        /// </summary>
        V = 0x40,

        /// <summary>
        /// Unused
        /// </summary>
        _ = 0x20,

        /// <summary>
        /// Break flag
        /// </summary>
        B = 0x10,

        /// <summary>
        /// Decimal mode flag
        /// </summary>
        D = 0x08,

        /// <summary>
        /// Interrupt disable flag
        /// </summary>
        I = 0x04,

        /// <summary>
        /// Zero flag
        /// </summary>
        Z = 0x02,

        /// <summary>
        /// Carry flag
        /// </summary>
        C = 0x01
    }

    public static class ProcessorStatusExtensions
    {
        public static ProcessorStatus Clr(this ProcessorStatus self, ProcessorStatus flag)
        {
            var copy = self;
            copy &= ~flag;
            return copy;
        }

        public static ProcessorStatus Set(this ProcessorStatus self, ProcessorStatus flag)
        {
            var copy = self;
            copy |= flag;
            return copy;
        }

        public static ProcessorStatus SetOrClrIf(this ProcessorStatus self, ProcessorStatus flag, bool set)
        {
            var copy = self;
            if (set)
            {
                copy |= flag;
            }
            else
            {
                copy &= ~flag;
            }

            return copy;
        }

        public static bool IsSet(this ProcessorStatus self, ProcessorStatus flag)
        {
            return (self & flag) == flag;
        }

        public static bool IsClr(this ProcessorStatus self, ProcessorStatus flag)
        {
            return (self & flag) == 0;
        }
    }
}
