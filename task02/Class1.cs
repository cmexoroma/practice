using System.Security.Cryptography.X509Certificates;

namespace task02;

public class Student
{
    public string Name { get; set; }
    public string Faculty { get; set; }
    public List<int> Grades { get; set; }
}

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students)
    {
        _students = students;
    }

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    {
        var res = _students.Where(s => s.Faculty == faculty);

        return res;
    }

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    {
        var res = _students.Where(s => s.Grades.Average() >= minAverageGrade);
        
        return res;
    }

    public IEnumerable<Student> GetStudentsOrderedByName()
    {
        var res = _students.OrderBy(s => s.Name);

        return res;
    }

    public ILookup<string, Student> GroupStudentsByFaculty()
    {
        var res = _students.ToLookup(s => s.Faculty);

        return res;
    }

    public string GetFacultyWithHighestAverageGrade()
    {
        var max = _students.Max(s => s.Grades.Average());
        var res = _students.Where(s => s.Grades.Average() == max).Select(s => s.Faculty).First();

        return res;
    }
}
