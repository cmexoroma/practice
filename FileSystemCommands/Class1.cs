using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private string Catalog { get; }

    public DirectorySizeCommand(string catalog)
    {
        Catalog = catalog;
    }

    public void Execute()
    {
        if (Directory.Exists(Catalog))
        {
            long size = 0;
            var dirInfo = new DirectoryInfo(Catalog);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            if(files.Count() == 0)
            {
                Console.WriteLine("Каталог пуст");
                return;
            }

            foreach (var fileInfo in files) size += fileInfo.Length;

            Console.WriteLine($"Заданный каталог весит {size} байт(-а)");
        }
    }
}

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
