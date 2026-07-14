using System.Data;

namespace task17;

public class ExectionHandler
{
    public static void Handle(ICommand command, Exception ex)
    {
        Console.WriteLine($"Команда {command.GetType().Name} вызвала ошибку: {ex.Message}");
    }
}
