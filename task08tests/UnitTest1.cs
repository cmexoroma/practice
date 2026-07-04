using Xunit;
using MetaDate;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void MetaDate_EmptyTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        Program.Main(Array.Empty<string>());

        string expectedString = "Укажите путь к DLL файлу в аргумент функции";

        Assert.Contains(expectedString, output.ToString());
    }

    [Fact]
    public void MetaDate_Test()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        Program.Main(new[] {typeof(DirectorySizeCommand).Assembly.Location});

        string expectedString1 = "Метод: Execute";
        string expectedString2 = "Поиск файлов по маске";
        string expectedString3 = "Размер каталога";
        string expectedString4 = "Версия: 1.2";

        Assert.Contains(expectedString1, output.ToString());
        Assert.Contains(expectedString2, output.ToString());
        Assert.Contains(expectedString3, output.ToString());
        Assert.Contains(expectedString4, output.ToString());
    }
}

