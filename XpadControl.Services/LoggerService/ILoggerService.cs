using System;

namespace XpadControl.Services.LoggerService
{
    public interface ILoggerService : IDisposable
    {
        /// <summary>
        /// Anything and everything you might want to know about
        /// a running block of code.
        /// </summary>
        void WriteVerboseLog(string logMessage);

        /// <summary>
        /// Internal system events that aren't necessarily
        /// observable from the outside.
        /// </summary>
        void WriteDebugLog(string logMessage);

        /// <summary>
        /// The lifeblood of operational intelligence - things
        /// happen.
        /// </summary>
        void WriteInformationLog(string logMessage);

        /// <summary>
        /// Service is degraded or endangered.
        /// </summary>
        void WriteWarningLog(string logMessage);

        /// <summary>
        /// Functionality is unavailable, invariants are broken
        /// or data is lost.
        /// </summary>
        void WriteErrorLog(string logMessage);

        /// <summary>
        /// If you have a pager, it goes off when one of these
        /// occurs.
        /// </summary>
        void WriteFatalLog(string logMessage);
    }
}
