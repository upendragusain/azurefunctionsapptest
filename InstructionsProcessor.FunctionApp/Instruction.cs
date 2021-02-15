using System;
using System.Collections.Generic;
using System.Text;

namespace InstructionsProcessor.FunctionApp
{
    public class Instruction
    {
        public double Number { get; private set; }
        public string Operation { get; private set; }

        public Instruction(string operation, double number)
        {
            Number = number;
            Operation = operation;
        }
    }
}
