using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task10
{
    public class LoadAllDll
    {
        public static void Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
            var dllFiles = Directory.GetFiles(path, "Class*.dll", SearchOption.AllDirectories);

            Dictionary<Type, List<string>> globalDependence = new Dictionary<Type, List<string>>();
            List<string> completed = new List<string>();
            List<string> loadedDll = new List<string>();

            foreach (var dll in dllFiles)
            {
                if (dll.Contains("obj") || loadedDll.Contains(Path.GetFileName(dll))) continue;
                Assembly ass = Assembly.LoadFile(dll);
                loadedDll.Add(Path.GetFileName(dll));

                foreach (var type in ass.GetTypes())
                {
                    if (type.IsClass && Attribute.IsDefined(type, typeof(PluginLoadAttribute)) && typeof(ICommand).IsAssignableFrom(type))
                    {
                        globalDependence[type] = type.GetCustomAttribute<PluginLoadAttribute>().Dependence.ToList();
                    }
                }
            }

            Dictionary<string, List<Type>> dependents = new Dictionary<string, List<Type>>();
            foreach (var keyValue in globalDependence)
            {
                foreach (var depName in keyValue.Value)
                {
                    if (!dependents.ContainsKey(depName)) dependents[depName] = new List<Type>();
                    dependents[depName].Add(keyValue.Key);
                }
            }

            Dictionary<Type, int> unresolvedCount = new Dictionary<Type, int>();
            foreach (var type in globalDependence.Keys)
            {
                unresolvedCount[type] = globalDependence[type].Count;
            }

            Queue<Type> ready = new Queue<Type>();
            foreach (var type in globalDependence.Keys)
            {
                if (unresolvedCount[type] == 0) ready.Enqueue(type);
            }

            while (ready.Count > 0)
            {
                Type type = ready.Dequeue();
                ICommand instance = (ICommand)Activator.CreateInstance(type);
                instance.Execute();
                completed.Add(type.Name);

                if (dependents.TryGetValue(type.Name, out var dependentTypes))
                {
                    foreach (var dependent in dependentTypes)
                    {
                        unresolvedCount[dependent]--;
                        if (unresolvedCount[dependent] == 0)
                            ready.Enqueue(dependent);
                    }
                }
            }

            if (completed.Count != loadedDll.Count) Console.WriteLine("Ошибка: Существует цикл зависимостей");
        }
    }
}
