using FolderSynchronizer.Services;
using FolderSynchronizer.Services.Interfaces;
using FolderSynchronizer.Utilities;

namespace FolderSynchronizer;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var options = ArgumentParser.Parse(args);

            IFolderSynchronizer synchronizer = new Services.FolderSynchronizer();

            synchronizer.Synchronize(
                options.SourcePath,
                options.ReplicaPath);

            Console.WriteLine("Synchronization completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}