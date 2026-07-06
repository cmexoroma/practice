namespace task10
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class PluginLoadAttribute : Attribute
    {
        public string[] Dependence { get; }

        public PluginLoadAttribute(string[] dependence)
        {
            Dependence = dependence;
        }
    }
}
