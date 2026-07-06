using System;
using System.Reflection;
using System.Collections.Generic;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        var res = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(m => m.Name);

        return res;
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        if (string.IsNullOrWhiteSpace(methodname)) throw new ArgumentNullException(nameof(methodname));

        MethodInfo? method = _type.GetMethod(methodname);

        if(method != null)
        {
            var res = method.GetParameters().Select(p => p.Name!).Append(method.ReturnType.Name);

            return res;
        }

        return Enumerable.Empty<string>();
    }

    public IEnumerable<string> GetAllFields()
    {
        var res = _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(f => f.Name);

        return res;
    }

    public IEnumerable<string> GetProperties()
    {
        var res = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);

        return res;
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return Attribute.IsDefined(_type, typeof(T));
    }
}
