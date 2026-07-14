using task17;
using System.Threading;
using Moq;
using Xunit;

namespace task17tests;

public class ServerThreadTest
{
    [Fact]
    public void TestCommandLong()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);
        var output = new StringWriter();

        Console.SetOut(output);

        for (int i = 1; i < 6; i++) server.EnqueueCommand(new TestCommand(i));
        
        thread.Start();
        Thread.Sleep(500);

        server.EnqueueCommand(new HardStop(server));

        thread.Join(500);

        var str = output.ToString().Replace("\r", "").Split("\n").ToList();
        str.Remove("");
        var ex = new List<string>();
        for (int i = 1; i < 4; i++)
        {
            for (int j = 1; j < 6; j++)
            {
                ex.Add($"Поток {j} вызов {i}");
            }
        }
        Assert.Equal(ex, str);
        Assert.False(thread.IsAlive);
    }
}
