using InstructionsProcessor.FunctionApp.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstructionsProcessor.FunctionApp.Concrete
{
    public class FileProcessor : IFileProcessor
    {
        public (Queue<Instruction> instructions, double seed) Process(Stream stream)
        {
            if (!stream.CanSeek || stream.Length <= 0)
                throw new ArgumentException(ErrorCodes.EMPTY_STREAM.ToString());

            StringBuilder stringBuilder = new StringBuilder(8);
            Queue<Instruction> instructionsQueue = new Queue<Instruction>();
            Instruction instruction = null;
            double number = 0;

            using (var reader = new StreamReader(stream))
            {
                while (stringBuilder.Append(reader.ReadLine()).Length > 0)
                {
                    var line_instruction = stringBuilder.ToString().Split();// regex may be?

                    if (line_instruction.Length < 2)
                        throw new ArgumentException(ErrorCodes.INVALID_INSTRUCTION.ToString());

                    if (!double.TryParse(line_instruction[1], out number))
                        throw new ArgumentException(ErrorCodes.INVALID_INSTRUCTION.ToString());

                    if (!line_instruction[0].Equals(InstructionsProcessor.ADD, StringComparison.OrdinalIgnoreCase)
                        && !line_instruction[0].Equals(InstructionsProcessor.DIVIDE, StringComparison.OrdinalIgnoreCase)
                        && !line_instruction[0].Equals(InstructionsProcessor.MULTIPLY, StringComparison.OrdinalIgnoreCase)
                        && !line_instruction[0].Equals(InstructionsProcessor.SUBTRACT, StringComparison.OrdinalIgnoreCase)
                        && !line_instruction[0].Equals(InstructionsProcessor.APPLY, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(ErrorCodes.INVALID_INSTRUCTION.ToString());
                    }

                    instruction = new Instruction(line_instruction[0], number);
                    instructionsQueue.Enqueue(instruction);
                    stringBuilder.Clear();
                }
            }

            // last operation must be apply
            if (instruction.Operation != InstructionsProcessor.APPLY)
                throw new ArgumentException(ErrorCodes.MISSING_APPLY_OPERATION.ToString());

            return (instructionsQueue, instruction.Number);
        }
    }
}
