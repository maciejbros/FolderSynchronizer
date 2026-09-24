namespace FolderSynchronizer.Models;

public class SynchronizationOptions
{
    public string SourcePath { get; init; } = string.Empty;
    public string ReplicaPath { get; init; } = string.Empty;
    public int IntervalSeconds { get; init; }
    public string LogFilePath { get; init; } = string.Empty;
}