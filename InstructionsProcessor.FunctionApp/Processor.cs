using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InstructionsProcessor.FunctionApp.Abstract;
using System;

namespace InstructionsProcessor.FunctionApp
{
    public class Processor
    {
        private readonly IFileProcessor _fileProcessor;
        private readonly IInstructionsProcessor _instructionsProcessor;

        public Processor(
            IFileProcessor fileProcessor,
            IInstructionsProcessor instructionsProcessor)
        {
            _fileProcessor = fileProcessor;
            _instructionsProcessor = instructionsProcessor;
        }

        [FunctionName("process")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("processing request...");

                var processedFile =
                    await Task.Run(() => _fileProcessor.Process(req.Body));

                var calculationResult = await Task.Run(() => _instructionsProcessor
                    .Process(processedFile.instructions, processedFile.seed));

                return new OkObjectResult(calculationResult);
            }
            catch (ArgumentException exception)
            {
                log.LogError(exception, exception.Message);
                return new BadRequestObjectResult(exception.Message);
            }
            catch (Exception exception)
            {
                log.LogError(exception, exception.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
