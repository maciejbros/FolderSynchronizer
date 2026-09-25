using FolderSynchronizer.Services.Interfaces;

namespace FolderSynchronizer.Services;

public class FolderSynchronizer : IFolderSynchronizer
{
    public void Synchronize(string sourcePath, string replicaPath)
    {
        EnsureReplicaDirectoryExists(replicaPath);

        SynchronizeDirectories(sourcePath, replicaPath);
    }

    private static void EnsureReplicaDirectoryExists(string replicaPath)
    {
        if (!Directory.Exists(replicaPath))
        {
            Directory.CreateDirectory(replicaPath);
        }
    }

    private static void SynchronizeDirectories(
        string sourcePath,
        string replicaPath)
    {
        SynchronizeFiles(sourcePath, replicaPath);
        SynchronizeSubdirectories(sourcePath, replicaPath);
        RemoveExtraFiles(sourcePath, replicaPath);
        RemoveExtraDirectories(sourcePath, replicaPath);
    }

    private static void SynchronizeFiles(
        string sourcePath,
        string replicaPath)
    {
        foreach (string sourceFile in Directory.GetFiles(sourcePath))
        {
            string fileName = Path.GetFileName(sourceFile);
            string replicaFile = Path.Combine(replicaPath, fileName);

            if (!File.Exists(replicaFile) ||
                !FilesAreIdentical(sourceFile, replicaFile))
            {
                File.Copy(sourceFile, replicaFile, overwrite: true);
            }
        }
    }

    private static void SynchronizeSubdirectories(
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
            }

            SynchronizeDirectories(sourceDirectory, replicaDirectory);
        }
    }

    private static void RemoveExtraFiles(
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
            }
        }
    }

    private static void RemoveExtraDirectories(
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