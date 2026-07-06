using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task10
{
    public class LoadAllDll
    {
        public static void Load() => Load(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

        public static void Load(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"Директория плагинов не найдена: {path}");

            var dllFiles = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);

            if (dllFiles.Length == 0) throw new FileNotFoundException($"Не найдено DLL-файлов плагинов в {path}");

            Dictionary<Type, List<string>> globalDependence = new Dictionary<Type, List<string>>();
            List<string> completed = new List<string>();
            List<string> loadedDll = new List<string>();

            foreach (var dll in dllFiles)
            {
                if (dll.Contains("obj") || loadedDll.Contains(Path.GetFileName(dll))) continue;

                Assembly ass;
                try
                {
                    ass = Assembly.LoadFile(dll);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException($"Ошибка загрузки сборки");
                }

                loadedDll.Add(Path.GetFileName(dll));

                Type[] types;
                try
                {
                    types = ass.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    throw new InvalidOperationException($"Ошибка загрузки типов из {ass.GetName().Name}");
                }

                foreach (var type in types)
                {
                    if (type.IsClass && Attribute.IsDefined(type, typeof(PluginLoadAttribute)) && typeof(ICommand).IsAssignableFrom(type))
                    {
                        globalDependence[type] = type.GetCustomAttribute<PluginLoadAttribute>()!.Dependence.ToList();
                    }
                }
            }

            var allTypeNames = new HashSet<string>(globalDependence.Keys.Select(t => t.Name));
            foreach (var keyValue in globalDependence)
            {
                foreach (var dep in keyValue.Value)
                {
                    if (!allTypeNames.Contains(dep)) throw new InvalidOperationException($"Плагин {keyValue.Key.Name} зависит от {dep}, но такой плагин не найден");
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

                ICommand instance;
                try
                {
                    instance = (ICommand)Activator.CreateInstance(type)!;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException($"Ошибка создания экземпляра плагина {type.Name}");
                }

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

            if (completed.Count != globalDependence.Count)
                throw new InvalidOperationException("Обнаружен цикл зависимостей плагинов");
        }
    }
}
