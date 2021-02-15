using System.Collections.Generic;

namespace InstructionsProcessor.FunctionApp.Abstract
{
    public interface IInstructionsProcessor
    {
        double Process(Queue<Instruction> instructions, double seed);
    }
}