using FolderSynchronizer.Services;
using FolderSynchronizer.Services.Interfaces;
using FolderSynchronizer.Utilities;

namespace FolderSynchronizer;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var options = ArgumentParser.Parse(args);

            ILogger logger = new FileLogger(options.LogFilePath);

            IFolderSynchronizer synchronizer =
                new Services.FolderSynchronizer(logger);

            using CancellationTokenSource cancellationTokenSource = new();

            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            logger.Log($"Starting synchronization every {options.IntervalSeconds} seconds.");

            synchronizer.Synchronize(
                options.SourcePath,
                options.ReplicaPath);

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(options.IntervalSeconds));

            try
            {
                while (await timer.WaitForNextTickAsync(cancellationTokenSource.Token))
                {
                    synchronizer.Synchronize(
                        options.SourcePath,
                        options.ReplicaPath);
                }
            }
            catch (OperationCanceledException)
            {
                logger.Log($"Synchronization stoped.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}