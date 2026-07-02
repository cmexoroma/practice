using System.Reflection;

namespace MetaDate;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Введите путь до библиотеки");

        string? pathDll = Console.ReadLine();
        if(File.Exists(pathDll))
        {
            Assembly ass = Assembly.LoadFrom(pathDll);
            if(ass != null)
            {
                var types = ass.GetTypes().Where(t => !t.IsSubclassOf(typeof(Attribute)));
                foreach(Type t in types)
                {
                    Console.WriteLine($"Класс: {t.Name}");

                    foreach(var m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                    {
                        Console.WriteLine($"\tМетод: {m.Name}");
                        Console.Write("\t\tПараметры: ");
                        var parameters = m.GetParameters();
                        if(parameters.Length > 0)
                        {
                            foreach(var param in parameters) Console.Write($"{param.Name} - {param.ParameterType} ");
                            Console.WriteLine();
                        }
                        else Console.WriteLine("-");

                        var dispNameAtt = m.GetCustomAttribute<DisplayNameAttribute>();
                        if(dispNameAtt != null) Console.WriteLine($"Атрибут метода: {dispNameAtt.DisplayName}");
                    }

                    foreach(var con in t.GetConstructors())
                    {
                        Console.WriteLine($"\tКонструктор: {con.Name}");
                        Console.Write("\t\tПараметры: ");
                        var parameters = con.GetParameters();
                        if(parameters.Length > 0)
                        {
                            foreach(var param in con.GetParameters()) Console.Write($"{param.Name} - {param.ParameterType} ");
                            Console.WriteLine();
                        }
                        else Console.WriteLine("-");
                    }

                    Console.WriteLine("\tАтрибуты:");

                    var dispNameAttr = t.GetCustomAttribute<DisplayNameAttribute>();
                    if(dispNameAttr != null) Console.WriteLine($"\t\t{dispNameAttr.DisplayName}");

                    var versionAttr = t.GetCustomAttribute<VersionAttribute>();
                    if(versionAttr != null) Console.WriteLine($"\t\tВерсия: {versionAttr.Major}.{versionAttr.Minor}");
                }
            }
        }
        else Console.WriteLine("Библиотека не найдена");
    }
}
