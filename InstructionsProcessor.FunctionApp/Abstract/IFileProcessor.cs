using System.Collections.Generic;
using System.IO;

namespace InstructionsProcessor.FunctionApp.Abstract
{
    public interface IFileProcessor
    {
        (Queue<Instruction> instructions, double seed) Process(Stream stream);
    }
}