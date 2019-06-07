using System;
using System.Threading;

namespace CarWashBooking.Logging
{
    internal class FileLoggerHelper
    {
        private string fileName;

        public FileLoggerHelper(string fileName)
        {
            this.fileName = fileName;
        }

        static ReaderWriterLock locker = new ReaderWriterLock();

        internal void InsertLog(LogEntry logEntry)
        {
            var directory = System.IO.Path.GetDirectoryName(fileName);

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                System.IO.File.AppendAllText(fileName, $"{logEntry.CreatedTime} {logEntry.EventId} {logEntry.LogLevel} {logEntry.Message}" + Environment.NewLine);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }

    }
}
