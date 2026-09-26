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

            ILogger logger = new FileLogger(options.LogFilePath);

            IFolderSynchronizer synchronizer =
                new Services.FolderSynchronizer(logger);

            synchronizer.Synchronize(
                options.SourcePath,
                options.ReplicaPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}