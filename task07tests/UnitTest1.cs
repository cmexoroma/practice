using Xunit;
using task07;
using System.Reflection;


namespace task07tests;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void PrintTypeInfo_LogTest()
    {
        var type = typeof(SampleClass);
        var output = new StringWriter();

        Console.SetOut(output);

        var expectedString1 = "Имя класса: Пример класса";
        var expectedString2 = "Версия класса: 1.0";
        var expectedString3 = "Класс имеет метод: TestMethod - Тестовый метод";
        var expectedString4 = "Класс имеет свойство: Number - Числовое свойство";

        type.PrintTypeInfo();
        
        Assert.Contains(expectedString1, output.ToString());
        Assert.Contains(expectedString2, output.ToString());
        Assert.Contains(expectedString3, output.ToString());
        Assert.Contains(expectedString4, output.ToString());
    }
}
