using System.Collections.Concurrent;

namespace task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
    private readonly Scheduler _scheduler = new();
    private Thread? _serverThread;
    private bool _isActive = true;
    private bool _isSoftStopRequested = false;

    public void HardStop()
    {
        _isActive = false;
    }

    public void SoftStop()
    {
        _isSoftStopRequested = true;
    }

    public void EnqueueCommand(ICommand command)
    {
        _queue.Add(command);
    }

    public bool IsCurrentThreadSever()
    {
        return Thread.CurrentThread == _serverThread;
    }

    private void ExecuteCommand(ICommand command)
    {
        try
        {
            command.Execute();
        }
        catch (Exception ex)
        {
            ExectionHandler.Handle(command, ex);
        }
    }

    public void ExecuteThread()
    {
        _serverThread = Thread.CurrentThread;

        while (_isActive)
        {
            if (_isSoftStopRequested && _queue.Count == 0 && !_scheduler.HasCommand()) break;

            if (_queue.TryTake(out var command, 20))
            {
                if (command is ILongCommand longCommand)
                {
                    ExecuteCommand(command);
                    if (!longCommand.IsCompleted) _scheduler.Add(command);
                }
                else
                {
                    ExecuteCommand(command);
                }
                continue;
            }

            if (!_isActive) break;

            if (_scheduler.HasCommand())
            {
                var actCommand = _scheduler.Select();

                ExecuteCommand(actCommand);

                if (actCommand is ILongCommand longCommand && longCommand.IsCompleted) _scheduler.Remove(actCommand);

                continue;
            }

            if (_queue.Count == 0 && !_scheduler.HasCommand()) Thread.Sleep(20);
        }
    }
}
