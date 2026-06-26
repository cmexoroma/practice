using Xunit;
using Moq;
using task05;

namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetMethodParams_ReturnCorrectParams()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var param = analyzer.GetMethodParams("Method");

        Assert.Contains("Void", param);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnCorrectProperies()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var prop = analyzer.GetProperties();

        Assert.Contains("Property", prop);
    }

    [Fact]
    public void HasAttribute_ReturnTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        bool res = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(res);
    }
}
