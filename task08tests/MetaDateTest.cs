using Xunit;
using FileSystemCommands;

namespace task08tests;

public class MetaDateTest
{
    [Fact]
    public void MetaDate_EmptyTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        MetaDate.Main(Array.Empty<string>());

        string expectedString = "Укажите путь к DLL файлу в аргумент функции";

        Assert.Contains(expectedString, output.ToString());
    }

    [Fact]
    public void MetaDate_DirectorySizeCommandTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        MetaDate.Main(new[] {typeof(DirectorySizeCommand).Assembly.Location});


        string expectedString1 = "Размер каталога";
        string expectedString2 = "Версия: 1.0";
        string expectedString3 = "Метод: Execute";

        Assert.Contains(expectedString1, output.ToString());
        Assert.Contains(expectedString2, output.ToString());
        Assert.Contains(expectedString3, output.ToString());
    }

    [Fact]
    public void MetaDate_FindFilesCommandTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        MetaDate.Main(new[] {typeof(FindFilesCommand).Assembly.Location});

        string expectedString1 = "Метод: Execute";
        string expectedString2 = "Поиск файлов по маске";
        string expectedString3 = "Версия: 1.2";

        Assert.Contains(expectedString1, output.ToString());
        Assert.Contains(expectedString2, output.ToString());
        Assert.Contains(expectedString3, output.ToString());
    }
}

