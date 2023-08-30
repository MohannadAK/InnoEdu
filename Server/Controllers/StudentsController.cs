
namespace ThirdApp.Server;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private string studentsAllocationPath = "App_Data\\Students";
    private List<Student> students = new();

    public StudentsController() => ReadStudentsFromFile();

    private void ReadStudentsFromFile()
    {
        if (System.IO.File.Exists($"{studentsAllocationPath}\\students.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{studentsAllocationPath}\\students.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            students = JsonSerializer.Deserialize<List<Student>>(fileContent) ?? new List<Student>();
        }
    }

    ~StudentsController()
    {
        FlushStudentsToFile();
    }

    [HttpPost]
    public void CreateStudent([FromBody] Student student)
    {
        if (student is null) throw new ArgumentNullException(nameof(student));

        students.Add(student);

        FlushStudentsToFile();
    }

    private void FlushStudentsToFile()
    {
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(studentsAllocationPath))
            directoryInfo = Directory.CreateDirectory(studentsAllocationPath);

        System.IO.File.WriteAllText($"{studentsAllocationPath}\\students.json", JsonSerializer.Serialize(students));
    }

    [HttpGet]
    public IEnumerable<Student> ReadAllStudents() => students;

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Student value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}