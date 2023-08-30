using System.Net.Http.Json;

namespace ThirdApp.Client;

public partial class Students
{
    public List<Student>? students;
    public Student student = new();

    protected override async Task OnInitializedAsync()
    {
        students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");

        await base.OnInitializedAsync();
    }

    protected async void OnFormSubmit()
    {
        student.Id = Guid.NewGuid();
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync<Student>("api/students", student);
        if (response.IsSuccessStatusCode)
        {
            students?.Add(student);
            student = new();
        }

        //students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");

        await InvokeAsync(StateHasChanged);
    }
}