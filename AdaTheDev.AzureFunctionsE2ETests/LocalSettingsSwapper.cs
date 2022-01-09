using System;
using System.IO;

namespace AdaTheDev.AzureFunctionsE2ETests
{
    internal class LocalSettingsSwapper
    {
        private readonly string _functionAppPath;
        private readonly string _localSettingsFile;
        private string _localSettingsBackupFile;

        public LocalSettingsSwapper(string functionAppPath)
        {
            _functionAppPath = functionAppPath;
            _localSettingsFile = Path.Combine(functionAppPath, "local.settings.json");
        }

        public void Swap(string settingsFileName)
        {
            _localSettingsBackupFile = _localSettingsFile + $".backup{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            File.Copy(_localSettingsFile, _localSettingsBackupFile);
            File.Copy(Path.Combine(_functionAppPath, settingsFileName), _localSettingsFile, true);
        }

        public void Restore()
        {
            if (!string.IsNullOrEmpty(_localSettingsBackupFile) && File.Exists(_localSettingsBackupFile))
            {
                File.Copy(_localSettingsBackupFile, _localSettingsFile, true);
                File.Delete(_localSettingsBackupFile);
                _localSettingsBackupFile = string.Empty;
            }
        }
    }
}
