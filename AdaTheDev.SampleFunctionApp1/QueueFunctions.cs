using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdaTheDev.SampleFunctionApp1
{
    public class QueueFunctions
    {
        private readonly MySettings _mySettings;

        public QueueFunctions(IOptions<MySettings> settings)
        {
            _mySettings = settings.Value;
        }

        [FunctionName("QueueOutput")]
        [return: Queue("AdaTheDevE2ETestQueue", Connection = "AzureWebJobsStorage")]
        public string QueueOutput([HttpTrigger]string uniqueValue, ILogger log)
        {
            return $"{_mySettings.Setting1}-{uniqueValue}";
        }
    }
}
