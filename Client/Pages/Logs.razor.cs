//using System.Net.Http.Json;

//namespace InnoEdu.Client;
//public partial class Logs
//{
//    int i = 0;
//    public List<Log>? logs;

//    protected async override Task OnInitializedAsync()
//    {
//        logs = await _httpClient.GetFromJsonAsync<List<Log>>("api/logs");

//        await base.OnInitializedAsync();
//    }
//}