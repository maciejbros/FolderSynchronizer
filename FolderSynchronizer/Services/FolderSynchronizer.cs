using FolderSynchronizer.Services.Interfaces;

namespace FolderSynchronizer.Services;

public class FolderSynchronizer : IFolderSynchronizer
{
    private readonly ILogger _logger;

    public FolderSynchronizer(ILogger logger)
    {
        _logger = logger;
    }

    public void Synchronize(string sourcePath, string replicaPath)
    {
        EnsureReplicaDirectoryExists(replicaPath);

        SynchronizeDirectories(sourcePath, replicaPath);
    }

    private void EnsureReplicaDirectoryExists(string replicaPath)
    {
        if (!Directory.Exists(replicaPath))
        {
            Directory.CreateDirectory(replicaPath);

            _logger.Log(
                $"Created directory: {replicaPath}");
        }
    }

    private void SynchronizeDirectories(
        string sourcePath,
        string replicaPath)
    {
        SynchronizeFiles(sourcePath, replicaPath);
        SynchronizeSubdirectories(sourcePath, replicaPath);
        RemoveExtraFiles(sourcePath, replicaPath);
        RemoveExtraDirectories(sourcePath, replicaPath);
    }

    private void SynchronizeFiles(
        string sourcePath,
        string replicaPath)
    {
        foreach (string sourceFile in Directory.GetFiles(sourcePath))
        {
            string fileName = Path.GetFileName(sourceFile);
            string replicaFile = Path.Combine(replicaPath, fileName);

            if (!File.Exists(replicaFile))
            {
                File.Copy(sourceFile, replicaFile);

                _logger.Log(
                    $"Copied file: {sourceFile} -> {replicaFile}");

                continue;
            }

            if (!FilesAreIdentical(sourceFile, replicaFile))
            {
                File.Copy(sourceFile, replicaFile, overwrite: true);

                _logger.Log(
                    $"Updated file: {replicaFile}");
            }
        }
    }

    private void SynchronizeSubdirectories(
        string sourcePath,
        string replicaPath)
    {
        foreach (string sourceDirectory in Directory.GetDirectories(sourcePath))
        {
            string directoryName = Path.GetFileName(sourceDirectory);
            string replicaDirectory =
                Path.Combine(replicaPath, directoryName);

            if (!Directory.Exists(replicaDirectory))
            {
                Directory.CreateDirectory(replicaDirectory);

                _logger.Log(
                    $"Created directory: {replicaDirectory}");
            }

            SynchronizeDirectories(sourceDirectory, replicaDirectory);
        }
    }

    private void RemoveExtraFiles(
        string sourcePath,
        string replicaPath)
    {
        foreach (string replicaFile in Directory.GetFiles(replicaPath))
        {
            string fileName = Path.GetFileName(replicaFile);
            string sourceFile = Path.Combine(sourcePath, fileName);

            if (!File.Exists(sourceFile))
            {
                File.Delete(replicaFile);

                _logger.Log(
                    $"Removed file: {replicaFile}");
            }
        }
    }

    private void RemoveExtraDirectories(
        string sourcePath,
        string replicaPath)
    {
        foreach (string replicaDirectory in Directory.GetDirectories(replicaPath))
        {
            string directoryName = Path.GetFileName(replicaDirectory);
            string sourceDirectory =
                Path.Combine(sourcePath, directoryName);

            if (!Directory.Exists(sourceDirectory))
            {
                Directory.Delete(replicaDirectory, recursive: true);

                _logger.Log(
                    $"Removed directory: {replicaDirectory}");
            }
        }
    }

    private static bool FilesAreIdentical(
        string sourceFile,
        string replicaFile)
    {
        FileInfo sourceInfo = new(sourceFile);
        FileInfo replicaInfo = new(replicaFile);

        if (sourceInfo.Length != replicaInfo.Length)
        {
            return false;
        }

        using FileStream sourceStream = File.OpenRead(sourceFile);
        using FileStream replicaStream = File.OpenRead(replicaFile);

        int sourceByte;
        int replicaByte;

        do
        {
            sourceByte = sourceStream.ReadByte();
            replicaByte = replicaStream.ReadByte();

            if (sourceByte != replicaByte)
            {
                return false;
            }
        }
        while (sourceByte != -1);

        return true;
    }
}