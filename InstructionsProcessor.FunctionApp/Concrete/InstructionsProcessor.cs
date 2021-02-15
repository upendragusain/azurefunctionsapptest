using InstructionsProcessor.FunctionApp.Abstract;
using System;
using System.Collections.Generic;

namespace InstructionsProcessor.FunctionApp.Concrete
{
    public class InstructionsProcessor : IInstructionsProcessor
    {
        public const string ADD = "add";
        public const string DIVIDE = "divide";
        public const string MULTIPLY = "multiply";
        public const string SUBTRACT = "subtract";

        public const string APPLY = "apply";

        public double Process(Queue<Instruction> instructions, double seed)
        {
            byte applyCounter = 0;
            foreach (var item in instructions)
            {
                switch (item.Operation)
                {
                    case var _ when item.Operation.Equals(ADD, StringComparison.OrdinalIgnoreCase):
                        seed = seed + item.Number;
                        break;
                    case var _ when item.Operation.Equals(DIVIDE, StringComparison.OrdinalIgnoreCase):
                        seed = item.Number == 0 ? throw new ArgumentException(ErrorCodes.DIVIDE_BY_ZERO.ToString())
                            : seed / item.Number;
                        break;
                    case var _ when item.Operation.Equals(MULTIPLY, StringComparison.OrdinalIgnoreCase):
                        seed = seed * item.Number;
                        break;
                    case var _ when item.Operation.Equals(SUBTRACT, StringComparison.OrdinalIgnoreCase):
                        seed = seed - item.Number;
                        break;
                    case var _ when item.Operation.Equals(APPLY, StringComparison.OrdinalIgnoreCase):
                        applyCounter++;
                        if(applyCounter >= 2)
                            throw new ArgumentException(ErrorCodes.MULTIPLE_APPLY_OPERATION.ToString());
                        break;
                    default:
                        break;
                }
            }

            return seed;
        }
    }
}
