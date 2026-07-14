using task17;
using System.Threading;
using Moq;
using Xunit;

namespace task17tests;

public class ServerThreadTest
{
    [Fact]
    public void LongCommandsTest()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);
        var longCommandA = new Mock<ILongCommand>();
        var cntA = 0;
        var longCommandB = new Mock<ILongCommand>();
        var cntB = 0;
        var softStop = new SoftStop(server);
        List<string> order = new();

        longCommandA.Setup(c => c.IsCompleted).Returns(() => cntA >=5);
        longCommandB.Setup(c => c.IsCompleted).Returns(() => cntB >= 3);
        longCommandA.Setup(c => c.Execute()).Callback(() =>
        {
            cntA++;
            order.Add("A");
        });
        longCommandB.Setup(c => c.Execute()).Callback(() =>
        {
            cntB++;
            order.Add("B");
        });

        server.EnqueueCommand(longCommandA.Object);
        server.EnqueueCommand(softStop);
        server.EnqueueCommand(longCommandB.Object);

        thread.Start();
        thread.Join(500);

        Assert.Equal(5, order.Count(x => x == "A"));
        Assert.Equal(3, order.Count(x => x == "B"));
        Assert.False(thread.IsAlive);
    }

    [Fact]
    public void LongCommandWhithHardStopTest()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);
        var longCommandA = new Mock<ILongCommand>();
        var cntA = 0;
        var longCommandB = new Mock<ILongCommand>();
        var cntB = 0;
        var hardStop = new HardStop(server);
        List<string> order = new();

        longCommandA.Setup(c => c.IsCompleted).Returns(() => cntA >= 5);
        longCommandB.Setup(c => c.IsCompleted).Returns(() => cntB >= 3);
        longCommandA.Setup(c => c.Execute()).Callback(() =>
        {
            cntA++;
            order.Add("A");
        });
        longCommandB.Setup(c => c.Execute()).Callback(() =>
        {
            cntB++;
            order.Add("B");
        });

        server.EnqueueCommand(longCommandA.Object);
        server.EnqueueCommand(hardStop);
        server.EnqueueCommand(longCommandB.Object);

        thread.Start();
        thread.Join(500);

        Assert.Equal(1, order.Count(x => x == "A"));
        Assert.Equal(0, order.Count(x => x == "B"));
        Assert.False(thread.IsAlive);
    }

    [Fact]
    public void LongCommandsWithICommandTest()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);
        var longCommandA = new Mock<ILongCommand>();
        var cntA = 0;
        var longCommandB = new Mock<ILongCommand>();
        var cntB = 0;
        var CommandC = new Mock<ICommand>();
        var softStop = new SoftStop(server);
        List<string> order = new();

        longCommandA.Setup(c => c.IsCompleted).Returns(() => cntA >= 5);
        longCommandB.Setup(c => c.IsCompleted).Returns(() => cntB >= 3);
        longCommandA.Setup(c => c.Execute()).Callback(() =>
        {
            cntA++;
            order.Add("A");
        });
        longCommandB.Setup(c => c.Execute()).Callback(() =>
        {
            cntB++;
            order.Add("B");
        });
        CommandC.Setup(c => c.Execute()).Callback(()=>
        {
            order.Add("C");
        });

        server.EnqueueCommand(longCommandA.Object);
        server.EnqueueCommand(softStop);
        server.EnqueueCommand(longCommandB.Object);
        server.EnqueueCommand(CommandC.Object);

        thread.Start();
        thread.Join(500);

        Assert.Equal(5, order.Count(x => x == "A"));
        Assert.Equal(3, order.Count(x => x == "B"));
        Assert.Equal(1, order.Count(x => x == "C"));
        Assert.False(thread.IsAlive);
    }

    [Fact]
    public void LongCommandShouldExecuteInCorrectOrder()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);
        var longCommandA = new Mock<ILongCommand>();
        var cntA = 0;
        var longCommandB = new Mock<ILongCommand>();
        var cntB = 0;
        var CommandC = new Mock<ICommand>();
        var softStop = new SoftStop(server);
        List<string> order = new();

        longCommandA.Setup(c => c.IsCompleted).Returns(() => cntA >= 5);
        longCommandB.Setup(c => c.IsCompleted).Returns(() => cntB >= 3);
        longCommandA.Setup(c => c.Execute()).Callback(() =>
        {
            cntA++;
            order.Add("A");
        });
        longCommandB.Setup(c => c.Execute()).Callback(() =>
        {
            cntB++;
            order.Add("B");
        });
        CommandC.Setup(c => c.Execute()).Callback(()=>
        {
            order.Add("C");
        });

        server.EnqueueCommand(longCommandA.Object);
        server.EnqueueCommand(softStop);
        server.EnqueueCommand(longCommandB.Object);
        server.EnqueueCommand(CommandC.Object);

        thread.Start();
        thread.Join(500);

        Assert.False(thread.IsAlive);
        Assert.Equal(new[] {"A", "B", "C", "A", "B", "A", "B", "A", "A"}, order);
    }
}
