using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner;

public class Program
{
    public static void Main()
    {
        string dllPath = Path.Combine(AppContext.BaseDirectory, "FileSystemCommands.dll");

        if(File.Exists(dllPath))
        {
            Assembly ass = Assembly.LoadFrom(dllPath);

            var types = ass.GetTypes().Where(c => typeof(ICommand).IsAssignableFrom(c)).ToList();

            Console.WriteLine("Типы реализующие интерфейс ICommand:"); 
            foreach(var t in types)
            {
                Console.WriteLine($"\t{t.Name}");
            }

            foreach(var t in types)
            {
                if(t.Name == "DirectorySizeCommand")
                {
                    
                    var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

                    if(Directory.Exists(testDir)) Directory.Delete(testDir, true);

                    Directory.CreateDirectory(testDir);

                    File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
                    File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");
                    File.WriteAllText(Path.Combine(testDir, "test3.txt"), "Lighter");
                    File.WriteAllText(Path.Combine(testDir, "test4.txt"), "Flash");

                    var directorySizeInstance = (ICommand)Activator.CreateInstance(t, testDir);

                    directorySizeInstance.Execute();

                    Directory.Delete(testDir, true);
                }

                else if(t.Name == "FindFilesCommand")
                {
                    var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

                    if(Directory.Exists(testDir)) Directory.Delete(testDir, true);

                    Directory.CreateDirectory(testDir);
                    var subTestDir = Path.Combine(testDir, "SubDir");
                    Directory.CreateDirectory(subTestDir);

                    File.WriteAllText(Path.Combine(testDir, "test1.log"), "Hello");
                    File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");
                    File.WriteAllText(Path.Combine(subTestDir, "test3.txt"), "Lighter");
                    File.WriteAllText(Path.Combine(subTestDir, "test4.log"), "Flash");

                    var findFilesInstance = (ICommand)Activator.CreateInstance(t, testDir, "*.txt");

                    findFilesInstance.Execute();

                    Directory.Delete(testDir, true);
                }
            }
        }
        else Console.WriteLine("Ошибка: dll файл не найден");
    }
}
