using task10;

[PluginLoad(new string[] {})]
public class ClassA : ICommand
{
    public void Execute() {Console.WriteLine("ClassA");}
}
