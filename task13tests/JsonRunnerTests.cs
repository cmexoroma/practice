using task13;
using JsonRunner;
using System.Text.Json;

namespace task13tests;

public class JsonRunnerTests
{
    [Fact]
    public void JsonHelper_SerializationIncludeAllProperiesAndCorrectValueAndUseCastomDateFormat()
    {
        Student student = new Student()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2000, 1, 2),
            Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
        };

        string json = JsonHelper.SerializeStudent(student);

        Assert.NotEmpty(json);
        Assert.Contains("\"FirstName\": \"Ivan\"", json);
        Assert.Contains("\"LastName\": \"Ivanov\"", json);
        Assert.Contains("\"BirthDate\": \"02.01.2000\"", json);
        Assert.Contains("\"Grades\":", json);
        Assert.Contains("\"Name\": \"Math\"", json);
        Assert.Contains("\"Grade\": 5", json);
        Assert.Contains("\"Name\": \"Physics\"", json);
        Assert.Contains("\"Grade\": 4", json);
    }

    [Fact]
    public void JsonHelper_SerializationDosnotIncludeNull()
    {
        Student student = new Student()
        {
            FirstName = null,
            LastName = "Ivanov",
            BirthDate = new DateTime(2000, 1, 2),
            Grades = null
        };

        string json = JsonHelper.SerializeStudent(student);

        Assert.DoesNotContain("\"FirstName\"", json);
        Assert.DoesNotContain("\"Grades\"", json);
    }

    [Fact]
    public void JsonHelper_NullException()
    {
        Student student = null;

        Assert.Throws<ArgumentNullException>( () => JsonHelper.SerializeStudent(student));
    }

    [Fact]
    public void JsonHelper_DeSerializeValidJson()
    {
        string json = """
        {
            "FirstName": "Petr",
            "LastName": "Petrov",
            "BirthDate": "15.06.2001",
            "Grades": [{"Name": "Math", "Grade": 5}]
        }
        """;

        Student student = JsonHelper.DeSerializeStudent(json);

        Assert.NotNull(student);
        Assert.Equal("Petr", student.FirstName);
        Assert.Equal("Petrov", student.LastName);
        Assert.Equal(new DateTime(2001, 06, 15), student.BirthDate);
        Assert.Single(student.Grades);
        Assert.Equal("Math", student.Grades[0].Name);
        Assert.Equal(5, student.Grades[0].Grade);
    }

    [Fact]
    public void JsonHelper_DeSerializeInvalidJson()
    {
        Assert.Throws<JsonException>( () => JsonHelper.DeSerializeStudent("Invalid"));
    }

    [Fact]
    public void JsonHelper_DeSerializeInvalidStudentJson()
    {
        string invalidStudentJson = """
        {
        "LastName": "Ivanov",
        "BirthDate": "02.01.2000"
        }
        """;

        Assert.Throws<InvalidOperationException>( () => JsonHelper.DeSerializeStudent(invalidStudentJson));
    }

    [Fact]
    public void JsonHelper_DeSerializeInvalidStudentBithDateJson()
    {
        string invalidStudentJson = """
        {
        "FirstName": "Ivan",
        "LastName": "Ivanov",
        "BirthDate": "20.01.2036",
        "Grades": [{"Name": "Math", "Grade": 5}]
        }
        """;

        Assert.Throws<InvalidOperationException>( () => JsonHelper.DeSerializeStudent(invalidStudentJson));
    }

    [Fact]
    public void JsonHelper_RoundTrip()
    {
        Student originalStudent = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2000, 1, 2),
            Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
        };

        string json = JsonHelper.SerializeStudent(originalStudent);
        Student result = JsonHelper.DeSerializeStudent(json);

        Assert.Equal(originalStudent.FirstName, result.FirstName);
        Assert.Equal(originalStudent.LastName, result.LastName);
        Assert.Equal(originalStudent.BirthDate, result.BirthDate);
        Assert.Equal(originalStudent.Grades.Count, result.Grades.Count);
        for (int i = 0; i < originalStudent.Grades.Count; i++)
        {
            Assert.Equal(originalStudent.Grades[i].Name, result.Grades[i].Name);
            Assert.Equal(originalStudent.Grades[i].Grade, result.Grades[i].Grade);
        }
    }

    [Fact]
    public void JsonHelper_SaveAndLoadFile()
    {
        Student student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2000, 1, 2),
            Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
        };

        string path = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, "test.json");

        JsonHelper.SaveJsonFile(student, path);
        var result = JsonHelper.LoadFromJson(path);

        Assert.Equal(student.FirstName, result.FirstName);
        Assert.Equal(student.LastName, result.LastName);
        Assert.Equal(student.BirthDate, result.BirthDate);
        Assert.Equal(student.Grades.Count, result.Grades.Count);
        for (int i = 0; i < student.Grades.Count; i++)
        {
            Assert.Equal(student.Grades[i].Name, result.Grades[i].Name);
            Assert.Equal(student.Grades[i].Grade, result.Grades[i].Grade);
        }

        Directory.Delete(Path.GetDirectoryName(path), true);
    }

    [Fact]
    public void Main_SerializeArg_CreatesFile()
    {
        string dir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(dir);
        var output = new StringWriter();
        Console.SetOut(output);

        Runner.Main(["serialize", dir]);

        string filePath = Path.Combine(dir, "jsonFile.json");
        Assert.True(File.Exists(filePath));

        string content = File.ReadAllText(filePath);
        Assert.Contains("Ivan", content);
        Assert.Contains("Ivanov", content);

        Directory.Delete(dir, true);
    }

    [Fact]
    public void Main_DeserializeArg_PrintsStudent()
    {
        string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var student = new Student
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            BirthDate = new DateTime(2000, 1, 2),
            Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
        };
        JsonHelper.SaveJsonFile(student, Path.Combine(dir, "jsonFile.json"));

        var output = new StringWriter();
        Console.SetOut(output);

        Runner.Main(["deserialize", dir]);

        string consoleOut = output.ToString();
        Assert.Contains("Ivan", consoleOut);
        Assert.Contains("Ivanov", consoleOut);
    }
}
