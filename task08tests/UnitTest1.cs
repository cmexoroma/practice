using FileSystemCommands;
using CommandRunner;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        if(Directory.Exists(testDir)) Directory.Delete(testDir, true);

        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var output = new StringWriter();

        Console.SetOut(output);

        command.Execute(); // Проверяем, что не возникает исключений

        string expected = "10 байт(-а)";

        Directory.Delete(testDir, true);

        Assert.Contains(expected, output.ToString());
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        if(Directory.Exists(testDir)) Directory.Delete(testDir, true);

        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        var output = new StringWriter();

        Console.SetOut(output);

        command.Execute(); // Должен найти 1 файл

        string expected = "1 файл(-ов)";

        Directory.Delete(testDir, true);

        Assert.Contains(expected, output.ToString());
    }

    [Fact]
    public void CommandRunner_Test()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        
        Program.Main();

        string expectedString1 = "22 байт(-а)";
        string expectedString2 = "2 файл(-ов)";

        Assert.Contains(expectedString1, output.ToString());
        Assert.Contains(expectedString2, output.ToString());
    }
}
