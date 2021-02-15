using InstructionsProcessor.FunctionApp;
using InstructionsProcessor.FunctionApp.Concrete;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    public class FileProcessorTest
    {
        private FileProcessor _fileProcessor;
        private readonly IReadOnlyList<string> instructions = 
            new List<string>() { "add", "divide", "multiply", "subtract" };
        readonly Random random = new Random();

        [SetUp]
        public void Setup()
        {
            _fileProcessor = new FileProcessor();
        }

        [Test]
        public void RaiseExceptionWhenStreamIsEmpty()
        {
            var emptyStream = GenerateStreamFromString(string.Empty);
            var ex = Assert.Throws<ArgumentException>(() => _fileProcessor.Process(emptyStream));
            Assert.IsTrue(ex.Message.Contains(ErrorCodes.EMPTY_STREAM.ToString()));
        }

        [Test]
        public void RaiseExceptionWhenMissingApplyOperation()
        {
            var emptyStream = GenerateStreamFromString("ADD 1");//string case test 
            var ex = Assert.Throws<ArgumentException>(() => _fileProcessor.Process(emptyStream));
            Assert.IsTrue(ex.Message.Contains(ErrorCodes.MISSING_APPLY_OPERATION.ToString()));
        }

        [Test]
        [TestCaseSource("InvalidTestData")]
        public void RaiseExceptionWhenInvalidInstruction(string data)
        {
            var stream = GenerateStreamFromString(data);
            var ex = Assert.Throws<ArgumentException>(() => _fileProcessor.Process(stream));
            Assert.IsTrue(ex.Message.Contains(ErrorCodes.INVALID_INSTRUCTION.ToString()));
        }

        [Test]
        [TestCaseSource("ValidTestData")]
        public void ReturnInstructionsWhenDataIsValid(string data, Queue<Instruction> instructions, int seed)
        {
            var stream = GenerateStreamFromString(data);
            var result = _fileProcessor.Process(stream);

            foreach (var instruction in result.instructions)
            {
                var expectedInstruction = instructions.Dequeue();
                Assert.AreEqual(expectedInstruction.Operation, instruction.Operation);
                Assert.AreEqual(expectedInstruction.Number, instruction.Number);
            }

            Assert.AreEqual(seed, result.seed);
        }

        private static IEnumerable<TestCaseData> InvalidTestData()
        {
            yield return new TestCaseData("                ")
            .SetName("Empty spaces only should return INVALID_INSTRUCTION exception");

            yield return new TestCaseData($"add 1{Environment.NewLine}divide{Environment.NewLine}apply 3")
            .SetName("Missing number should return INVALID_INSTRUCTION exception");

            yield return new TestCaseData($"invalid_opeartion 1{Environment.NewLine}apply 3")
            .SetName("Unrecognised operation should return INVALID_INSTRUCTION exception");
        }

        private static IEnumerable<TestCaseData> ValidTestData()
        {
            yield return new TestCaseData($"add 2{Environment.NewLine}multiply 3{Environment.NewLine}apply 3",
                new Queue<Instruction>(new[] { new Instruction("add", 2), new Instruction("multiply", 3), new Instruction("apply", 3) }),
                3)
            .SetName($"add 2{Environment.NewLine}multiply 3{Environment.NewLine}apply 3");

            yield return new TestCaseData($"multiply 9{Environment.NewLine}apply 5",
                new Queue<Instruction>(new[] { new Instruction("multiply", 9), new Instruction("apply", 5) }),
                5)
            .SetName($"multiply 9{Environment.NewLine}apply 5");
        }

        private Stream GenerateStreamFromString(string fileData)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(fileData));
        }
    }
}