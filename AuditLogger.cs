using System;

namespace Connectionmonitor
{

    public class AuditLogger
    {
        const string _logfilename = "connectionhistory.csv";
        private string _logFileFullname;

        public AuditLogger()
        {
            var dir = System.Environment.GetEnvironmentVariable("MONITORFILE");
            if (string.IsNullOrWhiteSpace(dir))
            {
                dir = ".";
            }
            _logFileFullname = $"{dir}/{_logfilename}";
            
        }
        public void LogStart()
        {

            var logdata = string.Format("{0},{1},{2}\n"
                    , DateTime.Now.ToString("dd-MM-yyyy"),
                    DateTime.Now.ToString("hh:mm:ss"),
                    "Started");

            LogMessage(logdata);

        }
        private void LogMessage(string msg)
        {
            try
            {
                System.IO.File.AppendAllText(_logFileFullname, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error writing to logfile ({_logFileFullname})] {ex.Message}");
            }
        }

        public void LogState(ConnectionState connState)
        {
            var logdata = string.Format("{0},{1},{2}\n"
                    , DateTime.Now.ToString("dd-MM-yyyy"),
                    DateTime.Now.ToString("hh:mm:ss"),
                    connState);

            LogMessage(logdata);
        }
    }
}
