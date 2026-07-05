using task10;

[PluginLoad(new string[] {"ClassA"})]
public class ClassB : ICommand
{
    public void Execute() {Console.WriteLine("ClassB");}
}
