namespace Logger.Shared;

public class LogEntry
{
    public DateTime Date { get; set; }
    public LogType Type { get; set; }
    public string? Data { get; set; }
}
