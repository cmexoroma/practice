using Xunit;
using task10;

namespace task10tests;

public class UnitTest1
{
    [Fact]
    public void Load_Test()
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
}
