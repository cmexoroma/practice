using System.Collections.Concurrent;

namespace task17;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
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

    public void ExecuteThread()
    {
        _serverThread = Thread.CurrentThread;
        while (_isActive)
        {
            if (_isSoftStopRequested && _queue.Count == 0) break;

            if (_queue.TryTake(out var command, 200))
            {
                try
                {
                    command.Execute();
                }
                catch(Exception ex)
                {
                    ExectionHandler.Handle(command, ex);
                }
            }
        }
    }

    public void EnqueueCommand(ICommand command)
    {
        _queue.Add(command);
    }

    public bool IsCurrentThreadSever()
    {
        return Thread.CurrentThread == _serverThread;
    }
}
