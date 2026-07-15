namespace task17;

public class SoftStop : ICommand
{
    private readonly ServerThread _serverThread;
    public SoftStop(ServerThread serverThread)
    {
        _serverThread = serverThread;
    }

    public void Execute()
    {
        if (!_serverThread.IsCurrentThreadSever()) throw new InvalidOperationException("Команда вызвана вне потока");

        _serverThread.SoftStop();
    }
}
