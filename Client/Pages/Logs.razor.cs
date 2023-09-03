namespace InnoEdu.Client;
public partial class Logs
{
    int i = 0;
    private List<LogEntry>? logEntries;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            logEntries = await _httpClient.GetFromJsonAsync<List<LogEntry>>("api/logs/alllogs");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during deserialization: {ex.Message}");
        }
    }
}