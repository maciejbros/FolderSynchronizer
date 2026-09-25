using FolderSynchronizer.Services.Interfaces;

namespace FolderSynchronizer.Services;

public class FileLogger : ILogger
{
    private readonly string _logFilePath;

    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;

        string? directory = Path.GetDirectoryName(_logFilePath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void Log(string message)
    {
        string logMessage =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        Console.WriteLine(logMessage);
        File.AppendAllText(
            _logFilePath,
            logMessage + Environment.NewLine);
    }
}