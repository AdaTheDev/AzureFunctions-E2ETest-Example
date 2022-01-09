using Azure;
using Azure.Data.Tables;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AdaTheDev.AzureFunctionsE2ETests
{
    [Collection(nameof(AzureFunctionsTestsCollection))]
	public class SampleFunctionTests
    {
		private readonly AzureFunctionsFixture _fixture;

		public SampleFunctionTests(AzureFunctionsFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
        public async Task SimpleE2ETest()
        {
			var uniqueId = Guid.NewGuid();
			var content = new StringContent(uniqueId.ToString()); 
			
			// Send in a request to HTTP endpoint in SampleFunctionApp1...
			var response = await _fixture.Client.PostAsync("api/QueueOutput", content);
						
			response.EnsureSuccessStatusCode();

			// Settings from local-e2e.settings.json in SampleFunctionApp1 should have been swapped in.
			// Lookup the value based on the E2E setting
			string lookupValue = "E2E-" + uniqueId.ToString();

			// For this example, we're just going to poll the Azure Storage Table directly to see if the record was created.			
			// To be tolerant of a delay in processing and therefore not finding the record straight away, using Polly retry
			// policy to check for the record every second, for up to 5 seconds.
            var policy = Policy.Handle<RequestFailedException>(x => x.Status == 404).WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(1));
			await policy.ExecuteAsync(async () =>
			{
				await _fixture.TableClient.GetEntityAsync<TableEntity>("Test", lookupValue);				
			});
		}
    }
}
