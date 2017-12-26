using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace C64Emu._6502
{
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
        public byte S;

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

        UInt64 _currentCycle;

        public Cpu(Memory memory)
        {
            Memory = memory;
            PC = 0x0000;
            S = 0xff;
            P = ProcessorStatus._;
            A = 0;
            X = 0;
            Y = 0;
            _currentInstructionState = InstructionState.FetchOpCode;
            _currentCycle = 0;
        }

        public void RunCycle()
        {
            _currentCycle++;

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
