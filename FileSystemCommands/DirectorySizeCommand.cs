using CommandLib;

namespace FileSystemCommands;

[DisplayName("Размер каталога")]
[Version(1,0)]
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
