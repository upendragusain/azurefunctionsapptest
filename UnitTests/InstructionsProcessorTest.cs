using InstructionsProcessor.FunctionApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using sut = InstructionsProcessor.FunctionApp.Concrete.InstructionsProcessor;

namespace UnitTests
{
    public class InstructionsProcessorTest
    {
        private sut _instructionsProcessor;
        readonly Random random = new Random();

        [SetUp]
        public void Setup()
        {
            _instructionsProcessor = new sut();
        }

        [Test]
        public void RaiseExceptionWhenMultipleApplyInstructions()
        {
            var exception = Assert.Throws<ArgumentException>(() => _instructionsProcessor.Process(new Queue<Instruction>(new[] {
                new Instruction("apPly", 2), new Instruction("apply", 3) }), 1));
            Assert.IsTrue(exception.Message.Contains(ErrorCodes.DIVIDE_BY_ZERO.ToString()));
        }

        [Test]
        public void DivideByZero()
        {
            var exception = Assert.Throws<ArgumentException>(() => _instructionsProcessor.Process(
                new Queue<Instruction>(new[] { new Instruction("DIVIDE", 0), new Instruction("apply", 3) }), 1));
            Assert.IsTrue(exception.Message.Contains(ErrorCodes.DIVIDE_BY_ZERO.ToString()));
        }

        [Test]
        [TestCaseSource("ValidTestData")]
        public void ReturnInstructionsWhenDataIsValid(
            Queue<Instruction> instructions, double seed, double expectedResult)
        {
            var actualtResult = _instructionsProcessor.Process(instructions, seed);
            Assert.AreEqual(expectedResult, actualtResult);
        }

        private static IEnumerable<TestCaseData> ValidTestData()
        {
            yield return new TestCaseData(
                new Queue<Instruction>(new[] { new Instruction("ADD", 2), new Instruction("multiply", 3), new Instruction("apply", 3) }),
                3,
                15)
            .SetName($"3 + 2 * 3 = 15");

            yield return new TestCaseData(
                new Queue<Instruction>(new[] { new Instruction("ADD", 2), new Instruction("add", 2), new Instruction("MulTIplY", 4),
                    new Instruction("divide", 2), new Instruction("subtract", 2), new Instruction("divide", 4),
                    new Instruction("aPPlY", 3) }),
                3,
                3)
            .SetName($"3 + 2 + 2 * 4 / 2 - 2 / 4 = 3");

            yield return new TestCaseData(
                new Queue<Instruction>(new[] {
                                new Instruction("divide", -0.5678),
                                new Instruction("add", 32.9978),
                                new Instruction("subtract", 89.1734),
                                new Instruction("multiply", 0.1234),
                                new Instruction("apply", 12.3456) }),
                12.3456,
                -9.6151388533145473758365621697781)
            .SetName($"decimal values check");
        }
    }
}