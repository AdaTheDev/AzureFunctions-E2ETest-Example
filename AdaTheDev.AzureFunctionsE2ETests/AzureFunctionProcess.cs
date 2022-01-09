using System;
using System.Diagnostics;
using System.Threading;

namespace AdaTheDev.AzureFunctionsE2ETests
{
    public class AzureFunctionProcess : IDisposable
    {
        private Process _funcHostProcess;
        private bool disposed;
        private bool _funcHostIsReady;        
        private readonly bool _useShellExecute;
        
        public AzureFunctionProcess(
            string dotnetExePath,
            string functionHostPath,
            string functionAppFolder,
            int port,
            bool useShellExecute = false)
        {
            _funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = dotnetExePath,
                    Arguments = $"\"{functionHostPath}\" start -p {port}",
                    WorkingDirectory = functionAppFolder,
                    RedirectStandardOutput = !useShellExecute,
                    UseShellExecute = useShellExecute
                }
            };
            _useShellExecute = useShellExecute;
        }
                
        public void Start(int timeoutSeconds = 15)
        {
            _funcHostProcess.OutputDataReceived += _funcHostProcess_OutputDataReceived;

            try
            {
                _funcHostProcess.Start();

                var stopwatch = Stopwatch.StartNew();

                if (!_useShellExecute)
                {
                    _funcHostProcess.BeginOutputReadLine();

                    while (!_funcHostProcess.HasExited && !_funcHostIsReady && stopwatch.ElapsedMilliseconds < (timeoutSeconds * 1000))
                    {
                        Thread.Sleep(1000);
                    }
                }
                else 
                {
                    Thread.Sleep(timeoutSeconds * 1000);
                    _funcHostIsReady = true;
                }
            }
            finally
            {
                _funcHostProcess.OutputDataReceived -= _funcHostProcess_OutputDataReceived;
            }

            if (!_funcHostIsReady)
            {
                throw new InvalidOperationException("The Azure Functions host did not start up within an acceptable time.");
            }
        }
        
        private void _funcHostProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data?.Contains("For detailed output, run func with --verbose flag") ?? false)
            {
                _funcHostIsReady = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_funcHostProcess != null)
                    {
                        if (!_funcHostProcess.HasExited)
                        {
                            _funcHostProcess.Kill();
                        }

                        _funcHostProcess.Dispose();
                    }
                }
                
                disposed = true;
            }
        }
        public void Dispose()
        {            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
