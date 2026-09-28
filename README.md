# Folder Synchronizer

A C# console application that synchronizes a source folder with a replica folder.

The application performs **one-way synchronization**, ensuring that the replica folder contains the same files and directories as the source folder.

## Features

* One-way synchronization from `source` to `replica`
* Periodic synchronization with a configurable interval
* Automatic creation of the replica directory
* File creation and copying
* Detection and updating of modified files
* Removal of files and directories that no longer exist in the source
* File content comparison
* Logging to both the console and a log file
* Command-line configuration
* Graceful shutdown using `Ctrl+C`

## Technologies

* C#
* .NET 8
* .NET Console Application
* `PeriodicTimer`
* `CancellationToken`

No third-party library implementing folder synchronization is used.

## Project Structure

```text
FolderSynchronizer/
│
├── Models/
│   └── SynchronizationOptions.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IFolderSynchronizer.cs
│   │   └── ILogger.cs
│   │
│   ├── FileLogger.cs
│   └── FolderSynchronizer.cs
│
├── Utilities/
│   └── ArgumentParser.cs
│
└── Program.cs
```

### Components

**Models**

Contains classes representing application configuration.

**Services**

Contains the main application logic, including folder synchronization and logging.

**Services/Interfaces**

Contains abstractions used by the application, such as `IFolderSynchronizer` and `ILogger`.

**Utilities**

Contains helper functionality such as command-line argument parsing and validation.

**Program.cs**

Responsible for application startup, dependency creation, periodic synchronization and graceful shutdown.

## How Synchronization Works

The synchronization is performed in one direction:

```text
Source
  │
  │
  ▼
Replica
```

The source folder is treated as the single source of truth.

During each synchronization cycle:

1. The replica directory is created if it does not exist.
2. Files that exist in the source but not in the replica are copied.
3. Existing files are compared by their contents.
4. Modified files are overwritten with the source version.
5. Subdirectories are recursively synchronized.
6. Files that exist only in the replica are removed.
7. Directories that exist only in the replica are removed recursively.

After synchronization, the replica should contain the same directory structure and file contents as the source.

## Configuration

The application is configured using command-line arguments:

```text
<source> <replica> <interval-seconds> <log-file>
```

### Arguments

| Argument           | Description                         |
| ------------------ | ----------------------------------- |
| `source`           | Path to the source directory        |
| `replica`          | Path to the replica directory       |
| `interval-seconds` | Synchronization interval in seconds |
| `log-file`         | Path to the log file                |

## Running the Application

From the project directory, run:

```bash
dotnet run -- "<source>" "<replica>" "<interval-seconds>" "<log-file>"
```

Example:

```bash
dotnet run -- "C:\Test\Source" "C:\Test\Replica" 10 "C:\Test\sync.log"
```

This configuration performs synchronization every **10 seconds**.

The same application can also be run from the compiled executable:

```bash
FolderSynchronizer.exe "C:\Test\Source" "C:\Test\Replica" 10 "C:\Test\sync.log"
```

## Logging

All synchronization operations are logged with a timestamp.

Example console output:

```text
[2026-09-25 10:15:32] Starting synchronization every 10 seconds.
[2026-09-25 10:15:32] Created directory: C:\Test\Replica
[2026-09-25 10:15:32] Copied file: C:\Test\Source\file.txt -> C:\Test\Replica\file.txt
[2026-09-25 10:15:32] Created directory: C:\Test\Replica\Documents
[2026-09-25 10:15:32] Copied file: C:\Test\Source\Documents\document.pdf -> C:\Test\Replica\Documents\document.pdf
```

The same messages are written to the configured log file.

The application logs operations such as:

* directory creation
* file copying
* file updating
* file removal
* directory removal
* synchronization start
* synchronization stop

## Graceful Shutdown

The application runs continuously until it is stopped with:

```text
Ctrl+C
```

A `CancellationToken` is used to stop the periodic synchronization gracefully.

## Requirements

* .NET 8 SDK
* Windows, Linux or macOS

## Build

To build the project:

```bash
dotnet build
```

To run the application:

```bash
dotnet run -- "<source>" "<replica>" "<interval-seconds>" "<log-file>"
```

## Test Task

This project was created as a solution to a C# programming test task requiring a program that periodically synchronizes a source folder with a replica folder.

The implementation focuses on clear separation of responsibilities, built-in .NET functionality and avoiding unnecessary third-party dependencies.
