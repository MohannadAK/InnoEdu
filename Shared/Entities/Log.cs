namespace Logger.Shared;

public static class Log
{
    public static DateTime LogDateTime { get; set; }
    public static LogType Type { get; set; }
    public static string? Data { get; set; }

    private static readonly string lineSeperator = "===========================================================================";
    private static readonly string allocationPath = "App_Data\\Logs";

    private static Dictionary<LogType, int> logTypeIndices = new Dictionary<LogType, int>
        {
            { LogType.Information, 1 },
            { LogType.Debug, 1 },
            { LogType.Error, 1 },
            { LogType.Critical, 1 }
        };

    public static async Task WriteInformation(string data)
    {
        await WriteLog(LogType.Information, data);
    }

    public static async Task WriteDebug(string data)
    {
        await WriteLog(LogType.Debug, data);
    }

    public static async Task WriteError(string data)
    {
        await WriteLog(LogType.Error, data);
    }

    public static async Task WriteCritical(string data)
    {
        await WriteLog(LogType.Critical, data);
    }

    public static async Task<string> ReadAllLogs() => await ReadAllLogTypes();

    public static async Task<string> ReadInformationLogs() => await ReadLogsFromDirectoryAsync("Information");

    public static async Task<string> ReadDebugLogs() => await ReadLogsFromDirectoryAsync("Debug");

    public static async Task<string> ReadErrorLogs() => await ReadLogsFromDirectoryAsync("Error");

    public static async Task<string> ReadCriticalLogs() => await ReadLogsFromDirectoryAsync("Critical");

    private static async Task WriteLog(LogType logType, string data)
    {
        string logDirectory = $"{allocationPath}\\{logType}";
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string logFilePath = $"{logDirectory}\\{logType}Logs_{logTypeIndices[logType]}.txt";

        long fileSize = GetFileSize(logFilePath);

        if (fileSize >= 10 * 1024) // 10KB
        {
            logTypeIndices[logType]++;
            logFilePath = $"{logDirectory}\\{logType}Logs_{logTypeIndices[logType]}.txt";
        }

        string content = $"Log date: {DateTime.Now}{Environment.NewLine}Log: {data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";

        await File.AppendAllTextAsync(logFilePath, content);
    }

    private static long GetFileSize(string filePath)
    {
        if (File.Exists(filePath))
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        return 0;
    }

    private static async Task<string> ReadAllLogTypes()
    {
        string informationLogs = await ReadInformationLogs();
        string debugLogs = await ReadDebugLogs();
        string errorLogs = await ReadErrorLogs();
        string criticalLogs = await ReadCriticalLogs();

        string allLogs =
            $"Information Logs:{Environment.NewLine}{informationLogs}{Environment.NewLine}" +
            $"Debug Logs:{Environment.NewLine}{debugLogs}{Environment.NewLine}" +
            $"Error Logs:{Environment.NewLine}{errorLogs}{Environment.NewLine}" +
            $"Critical Logs:{Environment.NewLine}{criticalLogs}{Environment.NewLine}";

        return allLogs;
    }

    private static async Task<string> ReadLogsFromDirectoryAsync(string subDirectory)
    {
        try
        {
            string logDirectory = $"{allocationPath}\\{subDirectory}";
            string[] logFiles = Directory.GetFiles(logDirectory, "*Logs*.txt");

            StringBuilder allLogContent = new();

            foreach (string logFile in logFiles)
            {
                string logContent = await File.ReadAllTextAsync(logFile);
                allLogContent.Append(logContent);
            }

            return allLogContent.ToString();
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }
}