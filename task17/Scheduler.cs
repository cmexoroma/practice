namespace task17;

public class Scheduler : IScheduler
{
    private readonly List<ICommand> _commands = new();
    private int _currentCommandIndex = 0;
    public bool HasCommand()
    {
        return _commands.Count > 0;
    }

    public ICommand Select()
    {
        if(!HasCommand()) throw new InvalidOperationException("В планировщике нет задач.");

        if(_currentCommandIndex >= _commands.Count) _currentCommandIndex = 0;

        ICommand selectCommand = _commands[_currentCommandIndex];
        _currentCommandIndex = (_currentCommandIndex + 1) % _commands.Count;

        return selectCommand;
    }

    public void Add(ICommand command)
    {
        _commands.Add(command);
    }

    public void Remove(ICommand command)
    {
        var removeIndex = _commands.IndexOf(command);
        if (removeIndex == -1) return;

        _commands.RemoveAt(removeIndex);

        _currentCommandIndex--;

        if (_currentCommandIndex < 0) _currentCommandIndex = 0;
    }
}
