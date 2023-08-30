using InnoEdu.Shared.Entities;

namespace Logger.Shared;

public static class Log
{
    public static DateTime LogDateTime { get; set; }
    public static LogType Type { get; set; }
    public static string? Data { get; set; }

    private static readonly string lineSeperator = "===========================================================================";
    private static readonly string allocationPath = "App_Data\\Logs";

    public static async Task WriteInformation(string data) => await WriteInformationLog(LogType.Information, data);

    public static async Task WriteDebug(string data) => await WriteDebugLog(LogType.Debug, data);

    public static async Task WriteError(string data) => await WriteErrorLog(LogType.Error, data);

    public static async Task WriteCritical(string data) => await WriteCriticalLog(LogType.Critical, data);

    public static async Task<string> ReadAllLogs() => await ReadAllLogTypes();

    public static async Task<string> ReadInformationLogs() => await ReadLogsFromDirectoryAsync("Information");

    public static async Task<string> ReadDebugLogs() => await ReadLogsFromDirectoryAsync("Debug");

    public static async Task<string> ReadErrorLogs() => await ReadLogsFromDirectoryAsync("Error");

    public static async Task<string> ReadCriticalLogs() => await ReadLogsFromDirectoryAsync("Critical");

    private static async Task WriteInformationLog(LogType logType, string data)
    {
        if (!Directory.Exists($"{allocationPath}\\Information"))
            _ = Directory.CreateDirectory($"{allocationPath}\\Information");

        string logPath = $"{allocationPath}\\Information\\{logType}Logs.txt";
        string content = $"Log date: {DateTime.Now}{Environment.NewLine}Log: {data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";
        await File.AppendAllTextAsync(logPath, content);
    }

    private static async Task WriteDebugLog(LogType logType, string data)
    {
        if (!Directory.Exists($"{allocationPath}\\Debug"))
            _ = Directory.CreateDirectory($"{allocationPath}\\Debug");

        string logPath = $"{allocationPath}\\Debug\\{logType}Logs.txt";
        string content = $"Log date: {DateTime.Now}{Environment.NewLine}Log: {data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";
        await File.AppendAllTextAsync(logPath, content);
    }

    private static async Task WriteErrorLog(LogType logType, string data)
    {
        if (!Directory.Exists($"{allocationPath}\\Error"))
            _ = Directory.CreateDirectory($"{allocationPath}\\Error");

        string logPath = $"{allocationPath}\\Error\\{logType}Logs.txt";
        string content = $"Log date: {DateTime.Now}{Environment.NewLine}Log: {data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";
        await File.AppendAllTextAsync(logPath, content);
    }

    private static async Task WriteCriticalLog(LogType logType, string data)
    {
        if (!Directory.Exists($"{allocationPath}\\Critical"))
            _ = Directory.CreateDirectory($"{allocationPath}\\Critical");

        string logPath = $"{allocationPath}\\Critical\\{logType}Logs.txt";
        string content = $"Log date: {DateTime.Now}{Environment.NewLine}Log: {data}{Environment.NewLine}{lineSeperator}{Environment.NewLine}";
        await File.AppendAllTextAsync(logPath, content);
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
            string[] logFiles = Directory.GetFiles(logDirectory, "*Logs.txt");

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