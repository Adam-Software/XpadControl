using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace XpadControl.Services.LoggerService
{
    public class LoggerService : ILoggerService
    {
        private readonly Logger mLogger;
        public LoggerService(IConfigurationRoot configuration) 
        {
             mLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public void Dispose()
        {
            mLogger?.Dispose();
        }

        public void WriteDebugLog(string logMessage)
        {
            mLogger?.Debug(logMessage);
        }

        public void WriteErrorLog(string logMessage)
        {
            mLogger?.Error(logMessage);
        }

        public void WriteFatalLog(string logMessage)
        {
            mLogger?.Error(logMessage);
        }

        public void WriteInformationLog(string logMessage)
        {
            mLogger?.Information(logMessage);
        }

        public void WriteVerboseLog(string logMessage)
        {
            mLogger?.Verbose(logMessage);
        }

        public void WriteWarningLog(string logMessage)
        {
            mLogger?.Warning(logMessage);
        }
    }
}
