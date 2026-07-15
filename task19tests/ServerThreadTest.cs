using task17;
using System.Threading;
using Moq;
using Xunit;
using ScottPlot;
using System.Text.RegularExpressions;

namespace task19tests;

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
        foreach(var s in str) Console.WriteLine(s);
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

        using (var writer = new StreamWriter(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "report.txt")))
        {
            if (ex.SequenceEqual(str)) writer.WriteLine("Задачи выполняются в верной последовательности");
            else writer.WriteLine("Задачи выполняются в неверной последовательности");
        }

        var globalSteps = 0;
        var executionLog = new List<(double Step, double TaskId)>();

        foreach (var l in str)
        {
            var mathces = Regex.Matches(l, @"\d+");

            if (mathces.Count >= 2)
            {
                globalSteps++;
                executionLog.Add((globalSteps, double.Parse(mathces[0].Value)));
            }
        }

        var plt = new Plot();

        var steps = executionLog.Select(x => x.Step).ToArray();
        var taskIds = executionLog.Select(x => x.TaskId).ToArray();

        var scatter = plt.Add.Scatter(steps, taskIds);
        scatter.LineWidth = 0;

        plt.Title("Сетка выполнения задач (Round Robin)");
        plt.XLabel("Сквозной шаг выполнения сервера");
        plt.YLabel("ID задачи");
        plt.Axes.Left.SetTicks(new double[] {1, 2, 3, 4, 5}, new string[] {"1", "2", "3", "4", "5"});

        plt.SavePng(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "round_robin.png"), 800, 600);
    }
}
