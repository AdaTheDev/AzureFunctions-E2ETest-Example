namespace AdaTheDev.SampleFunctionApp2
{
    public static partial class QueueProcessFunctions
    {
        public class MyPoco
        {
            public string PartitionKey { get; set; }

            public string RowKey { get; set; }
        }
    }
}
