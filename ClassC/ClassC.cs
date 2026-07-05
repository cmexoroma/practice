using task10;

[PluginLoad(new string[] {"ClassA", "ClassB"})]
public class ClassC : ICommand
{
    public void Execute() {Console.WriteLine("ClassC");}
}
