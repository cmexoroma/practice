using task13;

namespace JsonRunner;

public class Runner
{
    public static void Main(string[] args)
    {
        if (args is ["serialize", var serializePath])
        {
            var standartStudent = new Student()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                BirthDate = new DateTime(2000, 1, 2),
                Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
            };
            JsonHelper.SaveJsonFile(standartStudent, Path.Combine(serializePath, "jsonFile.json"));
            Console.WriteLine("Файл сохранён");
        }
        else if (args is ["deserialize", var DeserializePath])
        {
            var student = JsonHelper.LoadFromJson(Path.Combine(DeserializePath, "jsonFile.json"));
            Console.Write($"{student.FirstName} {student.LastName}, {student.BirthDate:dd.MM.yyyy}");
            foreach (var sub in student.Grades) Console.Write($" {sub.Name} {sub.Grade}");
        }
        else
        {
            Console.Write("Введите путь: ");
            string? inputPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputPath) || !Directory.Exists(inputPath))
            {
                Console.WriteLine("Путь не найден");
                return;
            }

            var standartStudent = new Student()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                BirthDate = new DateTime(2000, 1, 2),
                Grades = new List<Subject> { new Subject { Name = "Math", Grade = 5 }, new Subject { Name = "Physics", Grade = 4 } }
            };

            while (true)
            {
                Console.WriteLine("1 - Записать\n2 - Прочитать\n0 - Выход");
                switch (Console.ReadLine())
                {
                    case "1":
                        JsonHelper.SaveJsonFile(standartStudent, Path.Combine(inputPath, "jsonFile.json"));
                        break;
                    case "2":
                        var s = JsonHelper.LoadFromJson(Path.Combine(inputPath, "jsonFile.json"));
                        Console.WriteLine($"{s.FirstName} {s.LastName}, {s.BirthDate:dd.MM.yyyy}");
                        break;
                    case "0": 
                        return;
                    default:
                        Console.WriteLine("Некорректное значение");
                        break;
                }
            }
        }
    }
}
