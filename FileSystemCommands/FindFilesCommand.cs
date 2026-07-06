using CommandLib;

namespace FileSystemCommands;

[DisplayName("Поиск файлов по маске")]
[Version(1,2)]
public class FindFilesCommand : ICommand
{
    private string Catalog { get; }
    private string Mask { get; }

    public FindFilesCommand(string catalog, string mask)
    {
        Catalog = catalog;
        Mask = mask;
    }

    public void Execute()
    {
        if (Directory.Exists(Catalog))
        {
            var dirInfo = new DirectoryInfo(Catalog);
            var files = dirInfo.GetFiles(Mask, SearchOption.AllDirectories);

            Console.WriteLine($"Заданная директория имеет {files.Count()} файл(-ов) подходящих под маску {Mask}");
        }
    }
}
