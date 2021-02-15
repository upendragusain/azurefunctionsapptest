using InstructionsProcessor.FunctionApp.Abstract;
using InstructionsProcessor.FunctionApp.Concrete;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(InstructionsProcessor.FunctionApp.Startup))]

namespace InstructionsProcessor.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IFileProcessor, FileProcessor>();
            builder.Services.AddScoped<IInstructionsProcessor, Concrete.InstructionsProcessor>();
        }
    }
}
