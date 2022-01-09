using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AdaTheDev.SampleFunctionApp2
{
    public static partial class QueueProcessFunctions
    {

        [FunctionName("QueueProcessFunctions")]
        [return: Table("AdaTheDevE2ETestTable")]
        public static MyPoco Run([QueueTrigger("AdaTheDevE2ETestQueue", Connection = "AzureWebJobsStorage")]string uniqueValue, ILogger log)
        {
            return new MyPoco { PartitionKey = "Test", RowKey = uniqueValue };
        }
    }
}
