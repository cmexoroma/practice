namespace task17;

public class TestCommand : ILongCommand
{
    private readonly int _id;
    private readonly int _maxRuns;
    private int _counter = 0;

    public bool IsCompleted => _counter >= _maxRuns;

    public TestCommand(int id, int maxRuns = 3)
    {
        _id = id;
        _maxRuns = maxRuns;
    }

    public void Execute()
    {
        if (IsCompleted) return;

        Console.WriteLine($"Поток {_id} вызов {++_counter}");
    }
}
