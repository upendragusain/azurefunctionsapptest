using BenchmarkDotNet.Attributes;
using InstructionsProcessor.FunctionApp.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BenchmarkTests
{
    public class FileProcessorBenchmarkTests
    {
        private FileProcessor _fileProcessor;
        private IReadOnlyList<string> instructions = 
            new List<string>() { "add", "divide", "multiply", "subtract" };

        public FileProcessorBenchmarkTests()
        {
            _fileProcessor = new FileProcessor();
        }

        private Stream GetTestData(int numOfLines)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < numOfLines; i++)
            {
                sb.Append($"{instructions[random.Next(1, 4)]} {random.Next(1, 10)}{Environment.NewLine}");
            }
            sb.Append($"apply {random.Next(1, 10)}{Environment.NewLine}");

            return GenerateStreamFromString(sb.ToString());
        }

        private Stream GenerateStreamFromString(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }

        [Benchmark]
        [ArgumentsSource(nameof(Numbers))]
        public (Queue<InstructionsProcessor.FunctionApp.Instruction> instructions, double seed) test1(Stream stream)
        {
            //var x = $"add 2{Environment.NewLine}multiply 3{Environment.NewLine}apply 3";
            return _fileProcessor.Process(stream);
        }

        public IEnumerable<object[]> Numbers() // for multiple arguments it's an IEnumerable of array of objects (object[])
        {
            yield return new object[]{ GetTestData(10) };
        }
    }
}
