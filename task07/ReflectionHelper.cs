using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(this Type type)
    {
        var classDispNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (classDispNameAttr != null) Console.WriteLine($"Имя класса: {classDispNameAttr.DisplayName}");

        var classVersion = type.GetCustomAttribute<VersionAttribute>();
        if (classVersion != null) Console.WriteLine($"Версия класса: {classVersion.Major}.{classVersion.Minor}");

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        if (methods.Count() != 0)
        {
            foreach (var method in methods)
            {
                var methodDispNameAttr = method.GetCustomAttribute<DisplayNameAttribute>();
                if (methodDispNameAttr != null) Console.WriteLine($"Класс имеет метод: {method.Name} - {methodDispNameAttr.DisplayName}");
            }
        }
        else Console.WriteLine("Класс методов не имеет");


        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        if (properties.Count() != 0)
        {
            foreach (var prop in properties)
            {
                var propDispNameAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (propDispNameAttr != null) Console.WriteLine($"Класс имеет свойство: {prop.Name} - {propDispNameAttr.DisplayName}");
            }
        }
        else Console.WriteLine("Класс свойств не имеет");
    }
}
