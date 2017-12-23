using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

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

    public enum InstructionState
    {
        FetchOpCode,
        RunOp
    }

    public class Cpu
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Memory Memory;

        /// <summary>
        /// Program counter
        /// </summary>
        public UInt16 PC;

        /// <summary>
        /// Stack pointer
        /// </summary>
        public byte _s;

        /// <summary>
        /// Processor status register
        /// </summary>
        public ProcessorStatus P;

        /// <summary>
        /// Accumulator
        /// </summary>
        public byte A;

        /// <summary>
        /// Index register x
        /// </summary>
        public byte X;

        /// <summary>
        /// Index register y
        /// </summary>
        public byte Y;

        private Operand _currentOperand;

        private InstructionState _currentInstructionState;

        public Cpu(Memory memory)
        {
            Memory = memory;
            PC = 0x0000;
            _s = 0xff;
            P = 0;
            A = 0;
            X = 0;
            Y = 0;
            _currentInstructionState = InstructionState.FetchOpCode;
        }

        public void RunCycle()
        {
            if (_currentInstructionState == InstructionState.FetchOpCode)
            {
                var opCode = Memory.Mem[PC];
                if (!Operands.List.TryGetValue(opCode, out _currentOperand))
                {
                    _logger.Fatal($"Unknown operand for opCode {opCode}, pc={PC}.");
                    throw new NotImplementedException();
                }

                _currentInstructionState = InstructionState.RunOp;
            }

            _currentOperand = _currentOperand.Run(_currentOperand, this);

            if (_currentOperand.Tim > _currentOperand.MaxTim)
            {
                _currentInstructionState = InstructionState.FetchOpCode;
            }
        }
    }
}
