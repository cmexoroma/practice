using Xunit;
using task10;

namespace task10tests;

public class PluginLoadTests
{
    [Fact]
    public void Load_NormalTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        string expectedStringA = "ClassA";
        string expectedStringB = "ClassB";
        string expectedStringC = "ClassC";

        LoadAllDll.Load();

        Assert.Contains(expectedStringA, output.ToString());
        Assert.Contains(expectedStringB, output.ToString());
        Assert.Contains(expectedStringC, output.ToString());
    }

    [Fact]
    public void Load_NotExistsPathTest()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Govno");

        Assert.Throws<DirectoryNotFoundException>(() => LoadAllDll.Load(path));
    }

    [Fact]
    public void Load_EmptyPath()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "EmptyTest");
        Directory.CreateDirectory(path);

        Assert.Throws<FileNotFoundException>(() => LoadAllDll.Load(path));

        Directory.Delete(path, true);
    }
}
