namespace Logger.Shared;

public static class Log
{
    public static DateTime Date { get; set; }
    public static LogType Type { get; set; }
    public static string? Data { get; set; }

    private static readonly List<LogEntry> allLogEntries = new List<LogEntry>();

    private static readonly string allocationPath = "App_Data\\Logs";

    private static readonly Dictionary<LogType, int> logTypeIndices = new()
    {
            { LogType.Information, 1 },
            { LogType.Debug, 1 },
            { LogType.Error, 1 },
            { LogType.Critical, 1 }
        };

    public static async Task WriteInformation(string data) => await WriteLog(LogType.Information, data);

    public static async Task WriteDebug(string data) => await WriteLog(LogType.Debug, data);

    public static async Task WriteError(string data) => await WriteLog(LogType.Error, data);

    public static async Task WriteCritical(string data) => await WriteLog(LogType.Critical, data);

    public static async Task<string> ReadAllLogs() => await ReadAllLogTypes();

    public static async Task<string> ReadInformationLogs() => await ReadLogsFromDirectoryAsync("Information");

    public static async Task<string> ReadDebugLogs() => await ReadLogsFromDirectoryAsync("Debug");

    public static async Task<string> ReadErrorLogs() => await ReadLogsFromDirectoryAsync("Error");

    public static async Task<string> ReadCriticalLogs() => await ReadLogsFromDirectoryAsync("Critical");

    public static async Task WriteLog(LogType logType, string data)
    {
        string logDirectory = $"{allocationPath}\\{logType}";
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string logFilePath = $"{logDirectory}\\{logType}Logs_{logTypeIndices[logType]}.json";

        long fileSize = GetFileSize(logFilePath);

        if (fileSize >= 10 * 1024)
        {
            logTypeIndices[logType]++;
            logFilePath = $"{logDirectory}\\{logType}Logs_{logTypeIndices[logType]}.json";
        }

        var logEntry = new LogEntry
        {
            Date = DateTime.Now,
            Type = logType,
            Data = data
        };

        allLogEntries.Add(logEntry);

        List<LogEntry> logEntries;

        if (File.Exists(logFilePath))
        {
            string existingData = await File.ReadAllTextAsync(logFilePath);
            logEntries = JsonSerializer.Deserialize<List<LogEntry>>(existingData) ?? new List<LogEntry>();
        }
        else
        {
            logEntries = new List<LogEntry>();
        }

        logEntries.Add(logEntry);

        string jsonContent = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(logFilePath, jsonContent);

    }

    public static List<LogEntry> GetAllLogs()
    {
        return allLogEntries;
    }

    private static long GetFileSize(string filePath)
    {
        if (File.Exists(filePath))
        {
            FileInfo fileInfo = new(filePath);
            return fileInfo.Length;
        }
        return 0;
    }

    private static async Task<string> ReadAllLogTypes()
    {
        var informationLogs = await ReadLogsFromDirectoryAsync("Information");
        var debugLogs = await ReadLogsFromDirectoryAsync("Debug");
        var errorLogs = await ReadLogsFromDirectoryAsync("Error");
        var criticalLogs = await ReadLogsFromDirectoryAsync("Critical");

        var logsList = new List<LogEntry>();

        if (!string.IsNullOrEmpty(informationLogs))
        {
            var informationLogEntries = JsonSerializer.Deserialize<List<LogEntry>>(informationLogs);
            logsList.AddRange(informationLogEntries);
        }

        if (!string.IsNullOrEmpty(debugLogs))
        {
            var debugLogEntries = JsonSerializer.Deserialize<List<LogEntry>>(debugLogs);
            logsList.AddRange(debugLogEntries);
        }

        if (!string.IsNullOrEmpty(errorLogs))
        {
            var errorLogEntries = JsonSerializer.Deserialize<List<LogEntry>>(errorLogs);
            logsList.AddRange(errorLogEntries);
        }

        if (!string.IsNullOrEmpty(criticalLogs))
        {
            var criticalLogEntries = JsonSerializer.Deserialize<List<LogEntry>>(criticalLogs);
            logsList.AddRange(criticalLogEntries);
        }

        return JsonSerializer.Serialize(logsList, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    private static void AppendLogs(StringBuilder allLogs, string logType, string? logs)
    {
        if (allLogs.Length > 0)
        {
            allLogs.Remove(allLogs.Length - 2, 2);
            allLogs.AppendLine(",");
        }
        allLogs.Append(logType + ": ");
        if (logs != null)
        {
            allLogs.AppendLine(logs);
        }
        else
        {
            allLogs.AppendLine("No " + logType + " available.");
        }
    }

    private static async Task<string?> ReadLogsFromDirectoryAsync(string subDirectory)
    {
        try
        {
            string logDirectory = $"{allocationPath}\\{subDirectory}";
            string[] logFiles = Directory.GetFiles(logDirectory, "*Logs*.json");

            if (logFiles.Length == 0)
            {
                return "[]";
            }

            var logContentList = new List<string>();

            foreach (string logFile in logFiles)
            {
                string logContent = await File.ReadAllTextAsync(logFile);
                logContentList.Add(logContent);
            }

            string allLogContent = "[" + string.Join(",", logContentList) + "]";

            return allLogContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null; 
        }
    }

    public static async Task<List<LogEntry>> ReadJsonDataFromFileAsync(string filePath)
    {
        try
        {
            string jsonData = await File.ReadAllTextAsync(filePath);
            var logEntries = JsonSerializer.Deserialize<List<LogEntry>>(jsonData);

            ArgumentNullException.ThrowIfNull(logEntries);

            return logEntries;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON data: {ex.Message}");
            return new List<LogEntry>();
        }
    }
}