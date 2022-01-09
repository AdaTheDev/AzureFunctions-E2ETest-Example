using Azure.Data.Tables;
using System;
using System.Net.Http;

namespace AdaTheDev.AzureFunctionsE2ETests
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureFunctionsFixture : IDisposable
	{
        private readonly AzureFunctionProcess _functionApp1Process;
        private readonly AzureFunctionProcess _functionApp2Process;
        private readonly LocalSettingsSwapper _functionApp1LocalSettingsSwapper = null;

        public readonly HttpClient Client;

        public readonly TableClient TableClient;
                
        private bool _disposed;

        public AzureFunctionsFixture()
        {
            // Set these to the appropriate values for your environment. Recommend pushing these into a settings file. Not done here for brevity.
            var dotnetExePath = @""; // should be C:\Program Files\dotnet\dotnet.exe
            var functionHostPath = @""; // for me, it was in this path: C:\Users\{UserName}\AppData\Local\AzureFunctionsTools\Releases\4.8.0\cli_x64\func.dll
            var functionApp1Folder = @""; // path to AdaTheDev.SampleFunctionApp1 project folder;
            var functionApp1Port = 7071;
            var functionApp1E2eSettingsFileName = "local-e2e.settings.json"; // Keep this to switch in the E2E version of local.settings.json for Function App 1 (test pass).
            var functionApp2Folder = @""; // path to AdaTheDev.SampleFunctionApp2 project folder;;
            var functionApp2Port = 7072;
            
            var tableStorageConnection = "UseDevelopmentStorage=true";
            
            var processFactory = new AzureFunctionProcessFactory(dotnetExePath, functionHostPath);
						
            try
            {
                if (!string.IsNullOrWhiteSpace(functionApp1E2eSettingsFileName))
                {
                    _functionApp1LocalSettingsSwapper = new LocalSettingsSwapper(functionApp1Folder);
                    _functionApp1LocalSettingsSwapper.Swap(functionApp1E2eSettingsFileName);
                }
                _functionApp1Process = processFactory.Create(functionApp1Folder, functionApp1Port);
                _functionApp1Process.Start();

                _functionApp2Process = processFactory.Create(functionApp2Folder, functionApp2Port);
                _functionApp2Process.Start();
            }
            catch
            {
                _functionApp1Process?.Dispose();
                _functionApp2Process?.Dispose();
                _functionApp1LocalSettingsSwapper?.Restore();
                throw;
            }

            TableClient = new TableClient(tableStorageConnection, "AdaTheDevE2ETestTable");
            TableClient.CreateIfNotExists();

            this.Client = new HttpClient();
            this.Client.BaseAddress = new Uri($"http://localhost:{functionApp1Port}");
        }
             
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.Client?.Dispose();

                    _functionApp1Process?.Dispose();
                    _functionApp2Process?.Dispose();
                    _functionApp1LocalSettingsSwapper?.Restore();
                }
                
                _disposed = true;
            }
        }

        public void Dispose()
        {            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
