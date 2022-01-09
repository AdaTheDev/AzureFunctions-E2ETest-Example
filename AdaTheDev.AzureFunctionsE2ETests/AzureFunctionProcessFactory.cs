namespace AdaTheDev.AzureFunctionsE2ETests
{
    public class AzureFunctionProcessFactory
    {
        private readonly string _dotnetExePath;
        private readonly string _functionHostPath;

        public AzureFunctionProcessFactory(
           string dotnetExePath,
           string functionHostPath)
        {
            _dotnetExePath = dotnetExePath;
            _functionHostPath = functionHostPath;
        }

        public AzureFunctionProcess Create(string functionAppFolder, int port, bool useShellExecute = false)
        {
            return new AzureFunctionProcess(_dotnetExePath, _functionHostPath, functionAppFolder, port, useShellExecute);
        }
    }
}
