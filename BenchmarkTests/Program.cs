using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace BenchmarkTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FileProcessorBenchmarkTests>();
        }
        //static void Main(string[] args) => 
        //    BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
    }
}
