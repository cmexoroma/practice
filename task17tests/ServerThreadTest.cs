using task17;
using System.Threading;
using Moq;
using Xunit;

namespace task17tests;

public class ServerThreadTest
{
    [Fact]
    public void SoftStopTest()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);

        var mockCommandA = new Mock<ICommand>();
        var mockCommandB = new Mock<ICommand>();
        var softStop = new SoftStop(server);

        server.EnqueueCommand(mockCommandA.Object);
        server.EnqueueCommand(softStop);
        server.EnqueueCommand(mockCommandB.Object);

        thread.Start();
        thread.Join(500);

        mockCommandA.Verify(c => c.Execute(), Times.Once());
        mockCommandB.Verify(c => c.Execute(), Times.Once());
        Assert.False(thread.IsAlive);
    }

     [Fact]
    public void HardStopTest()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);

        var mockCommandA = new Mock<ICommand>();
        var mockCommandB = new Mock<ICommand>();
        var hardStop = new HardStop(server);

        server.EnqueueCommand(mockCommandA.Object);
        server.EnqueueCommand(hardStop);
        server.EnqueueCommand(mockCommandB.Object);

        thread.Start();
        thread.Join(500);

        mockCommandA.Verify(c => c.Execute(), Times.Once());
        mockCommandB.Verify(c => c.Execute(), Times.Never());
        Assert.False(thread.IsAlive);
    }

    [Fact]
    public void ExectionHandlerTest()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);

        var mockCommandA = new Mock<ICommand>();
        var mockCommandB = new Mock<ICommand>();
        mockCommandB.Setup(c => c.Execute()).Throws(new Exception("Ошибка"));
        var mockCommandC = new Mock<ICommand>();
        var softStop = new SoftStop(server);

        server.EnqueueCommand(mockCommandA.Object);
        server.EnqueueCommand(softStop);
        server.EnqueueCommand(mockCommandB.Object);
        server.EnqueueCommand(mockCommandC.Object);

        thread.Start();
        thread.Join(500);

        mockCommandA.Verify(c => c.Execute(), Times.Once);
        mockCommandB.Verify(c => c.Execute(), Times.Once);
        mockCommandC.Verify(c => c.Execute(), Times.Once);
        Assert.False(thread.IsAlive);
        Assert.Contains("Команда ICommandProxy вызвала ошибку: Ошибка", output.ToString());
    }

    [Fact]
    public void HardStopFromMainThreadTest()
    {
        var server = new ServerThread();
        var hardStop = new HardStop(server);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
    }

    [Fact]
    public void SoftStopFromMainThreadTest()
    {
        var server = new ServerThread();
        var softStop = new SoftStop(server);

        Assert.Throws<InvalidOperationException>(() => softStop.Execute());
    }
}
