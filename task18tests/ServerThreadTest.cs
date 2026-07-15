using task17;
using System.Threading;
using Moq;
using Xunit;
using System.Diagnostics;
using ScottPlot;

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

    [Fact]
    public void GraphGenerate()
    {
        var server = new ServerThread();
        var thread = new Thread(server.ExecuteThread);

        var task1Points = new List<(double Time, double Progress)>();
        var task2Points = new List<(double Time, double Progress)>();
        var task3Points = new List<(double Time, double Progress)>();

        var task1Mock = new Mock<ILongCommand>();
        var task2Mock = new Mock<ILongCommand>();
        var task3Mock = new Mock<ILongCommand>();

        int t1Step = 0;
        int t2Step = 0;
        int t3Step = 0;

        task1Mock.Setup(c => c.IsCompleted).Returns(() => t1Step >= 2);
        task2Mock.Setup(c => c.IsCompleted).Returns(() => t2Step >= 3);
        task3Mock.Setup(c => c.IsCompleted).Returns(() => t3Step >= 5);

        var stopwatch = Stopwatch.StartNew();

        task1Mock.Setup(m => m.Execute()).Callback(() =>
        {
            t1Step++;
            Thread.Sleep(15);
            task1Points.Add((stopwatch.Elapsed.TotalMilliseconds, (double)t1Step / 2 * 100));
        });

        task2Mock.Setup(m => m.Execute()).Callback(() =>
        {
            t2Step++;
            Thread.Sleep(20);
            task2Points.Add((stopwatch.Elapsed.TotalMilliseconds, (double)t2Step / 3 * 100));
        });

        task3Mock.Setup(m => m.Execute()).Callback(() =>
        {
            t3Step++;
            Thread.Sleep(10);
            task3Points.Add((stopwatch.Elapsed.TotalMilliseconds, (double)t3Step / 5 * 100));
        });

        server.EnqueueCommand(task1Mock.Object);
        server.EnqueueCommand(task2Mock.Object);
        server.EnqueueCommand(task3Mock.Object);
        server.EnqueueCommand(new SoftStop(server));

        thread.Start();
        thread.Join(500);

        var plot = new Plot();
        plot.Title("Диаграмма прогресса выполнения задач (Round Robin)");
        plot.XLabel("Время (мс)");
        plot.YLabel("Прогресс (%)");

        double[] xs1 = new double[task1Points.Count + 1];
        double[] ys1 = new double[task1Points.Count + 1];
        xs1[0] = 0;
        ys1[0] = 0;
        for (int i = 1; i < task1Points.Count + 1; i++)
        {
            xs1[i] = task1Points[i - 1].Time;
            ys1[i] = task1Points[i - 1].Progress;
        }
        var s1 = plot.Add.Scatter(xs1, ys1);
        s1.LegendText = "Задача 1";
        s1.LineWidth = 2;
        s1.MarkerSize = 8;

        double[] xs2 = new double[task2Points.Count + 1];
        double[] ys2 = new double[task2Points.Count + 1];
        xs2[0] = 0;
        ys2[0] = 0;
        for (int i = 1; i < task2Points.Count + 1; i++)
        {
            xs2[i] = task2Points[i - 1].Time;
            ys2[i] = task2Points[i - 1].Progress;
        }
        var s2 = plot.Add.Scatter(xs2, ys2);
        s2.LegendText = "Задача 2";
        s2.LineWidth = 2;
        s2.MarkerSize = 8;

        double[] xs3 = new double[task3Points.Count + 1];
        double[] ys3 = new double[task3Points.Count + 1];
        xs3[0] = 0;
        ys3[0] = 0;
        for (int i = 1; i < task3Points.Count + 1; i++)
        {
            xs3[i] = task3Points[i - 1].Time;
            ys3[i] = task3Points[i - 1].Progress;
        }
        var s3 = plot.Add.Scatter(xs3, ys3);
        s3.LegendText = "Задача 3";
        s3.LineWidth = 2;
        s3.MarkerSize = 8;

        plot.ShowLegend();

        string filePathPNG = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "progress_chart.png");
        string filePathTXT = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "report.txt");
        plot.SavePng(filePathPNG, 800, 600);

        using(var writer = new StreamWriter(filePathTXT))
        {
            writer.WriteLine($"Задача 1 выполнилась за {task1Points.Last().Time} мс");
            writer.WriteLine($"Задача 2 выполнилась за {task2Points.Last().Time} мс");
            writer.WriteLine($"Задача 3 выполнилась за {task3Points.Last().Time} мс");
        }
    }
}
