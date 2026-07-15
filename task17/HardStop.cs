using System.Threading;

namespace task17;

public class HardStop : ICommand
{
    private readonly ServerThread _serverThread;
    public HardStop(ServerThread serverThread)
    {
        _serverThread = serverThread;
    }

    public void Execute()
    {
        if (!_serverThread.IsCurrentThreadSever()) throw new InvalidOperationException("Команда вызвана вне потока");

        _serverThread.HardStop();
    }
}
