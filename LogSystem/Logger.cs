using System;
using System.IO;

namespace LogSystem
{
    public class Logger
    {
        public readonly string LogFileName = "logs.txt";
        public readonly string LogDirectoryName = Directory.GetCurrentDirectory();
        public string LogPath => $@"{LogDirectoryName}/{LogFileName}";

        public static Logger Instance => _Instance == null ? _Instance = new Logger() : _Instance;
        public static Logger _Instance;


        private Logger()
        {
            InitFileSystem();
        }

        public void WriteDebug(string message)
        {
            WriteMessage("Debug  ", message);
        }

        public void WriteWarning(string message)
        {
            WriteMessage("Warning", message);
        }

        public void WriteError(string message)
        {
            WriteMessage("ERROR  ", message);
        }

        private void WriteMessage(string level, string message)
        {
            File.WriteAllText(LogPath, $"[{DateTime.Now:MM/dd hh:mm:ss}] {level}: {message}");
        }

        private void InitFileSystem()
        {
            if (!File.Exists(LogPath))
            {
                File.Create(LogPath).Dispose();
            }
        }

    }
}
