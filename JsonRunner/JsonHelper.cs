using System.Runtime.InteropServices;
using System.Text.Json;
using task13;

namespace JsonRunner;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        Converters = { new DateTimeConverter() }
    };

    public static string SerializeStudent(Student student)
    {
        if (student is null) throw new ArgumentNullException("Student is null");
        return JsonSerializer.Serialize(student, options);
    }

    public static Student DeSerializeStudent(string json)
    {
        if (json is null) throw new ArgumentNullException("Json in null");

        Student student = JsonSerializer.Deserialize<Student>(json, options);

        if (student is null) throw new NullReferenceException("Не удалось десериализовать файл");

        if (string.IsNullOrWhiteSpace(student.FirstName)) throw new InvalidOperationException("Firstname is invalid");
        if (string.IsNullOrWhiteSpace(student.LastName)) throw new InvalidOperationException("Lastname is invalid");
        if (student.BirthDate == default || student.BirthDate > DateTime.Today) throw new InvalidOperationException("BirthDate is invalid");
        if (student.Grades is null) throw new InvalidOperationException("Grades is invalid");
        foreach (var sub in student.Grades)
        {
            if (string.IsNullOrWhiteSpace(sub.Name)) throw new InvalidOperationException("Subject name is invalid");
            if (sub.Grade < 2 || sub.Grade > 5) throw new InvalidOperationException("Subject grade is invalid");
        }

        return student;
    }

    public static void SaveJsonFile(Student student, string path)
    {
        if (student is null) throw new ArgumentNullException("Student is null");
        if (path is null) throw new ArgumentNullException("Path is null");

        File.WriteAllText(path, SerializeStudent(student));
    }

    public static Student LoadFromJson(string path)
    {
        if (path is null) throw new ArgumentNullException("Path is null");

        return DeSerializeStudent(File.ReadAllText(path));
    }
}

