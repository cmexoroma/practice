using System.Reflection;

namespace task07;

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method | System.AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}

[System.AttributeUsage(System.AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    [DisplayName("Тестовый метод")]
    public void TestMethod() {}
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(this Type type)
    {
        var classDispNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if(classDispNameAttr != null) Console.WriteLine($"Имя класса: {classDispNameAttr.DisplayName}");

        var classVersion = type.GetCustomAttribute<VersionAttribute>();
        if(classVersion != null) Console.WriteLine($"Версия класса: {classVersion.Major}.{classVersion.Minor}");

        foreach(var method in type.GetMethods())
        {
            var methodDispNameAttr = method.GetCustomAttribute<DisplayNameAttribute>();
            if(methodDispNameAttr != null) Console.WriteLine($"Класс имеет метод: {method.Name} - {methodDispNameAttr.DisplayName}");
        }

        foreach(var prop in type.GetProperties())
        {
            var propDispNameAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
            if(propDispNameAttr != null) Console.WriteLine($"Класс имеет свойство: {prop.Name} - {propDispNameAttr.DisplayName}");
        }
    }
}
