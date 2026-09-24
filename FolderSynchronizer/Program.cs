using FolderSynchronizer.Utilities;

namespace FolderSynchronizer;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var options = ArgumentParser.Parse(args);

            Console.WriteLine("Folder Synchronizer");
            Console.WriteLine($"Source: {options.SourcePath}");
            Console.WriteLine($"Replica: {options.ReplicaPath}");
            Console.WriteLine($"Interval: {options.IntervalSeconds} seconds");
            Console.WriteLine($"Log file: {options.LogFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}