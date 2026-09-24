using FolderSynchronizer.Models;

namespace FolderSynchronizer.Utilities;

public static class ArgumentParser
{
    private const int ExpectedArgumentsCount = 4;

    public static SynchronizationOptions Parse(string[] args)
    {
        if (args.Length != ExpectedArgumentsCount)
        {
            throw new ArgumentException(
                "Usage: FolderSynchronizer <source> <replica> <interval-seconds> <log-file>");
        }

        string sourcePath = args[0];
        string replicaPath = args[1];
        string logFilePath = args[3];

        if (!int.TryParse(args[2], out int intervalSeconds) || intervalSeconds <= 0)
        {
            throw new ArgumentException(
                "Synchronization interval must be a positive integer.");
        }

        ValidateSourcePath(sourcePath);
        ValidateReplicaPath(replicaPath);

        return new SynchronizationOptions
        {
            SourcePath = Path.GetFullPath(sourcePath),
            ReplicaPath = Path.GetFullPath(replicaPath),
            IntervalSeconds = intervalSeconds,
            LogFilePath = Path.GetFullPath(logFilePath)
        };
    }

    private static void ValidateSourcePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Source path cannot be empty.");
        }

        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(
                $"Source directory does not exist: {path}");
        }
    }

    private static void ValidateReplicaPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Replica path cannot be empty.");
        }
    }
}