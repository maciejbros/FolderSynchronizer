namespace FolderSynchronizer.Services.Interfaces;

public interface IFolderSynchronizer
{
    void Synchronize(string sourcePath, string replicaPath);
}