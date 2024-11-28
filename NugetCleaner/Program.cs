using System;
using System.IO;
using System.Linq;
using NuGet.Versioning;

namespace NugetCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Укажите путь к каталогу в аргументах командной строки.");
                return;
            }

            foreach (string arg in args)
            {
                string rootDirectory = arg.Replace("\"", "");

                if (!Directory.Exists(rootDirectory))
                {
                    Console.WriteLine($"Каталог {rootDirectory} не существует.");
                    return;
                }

                try
                {
                    foreach (string packageDir in Directory.GetDirectories(rootDirectory))
                    {
                        Console.WriteLine($"Обработка пакета: {Path.GetFileName(packageDir)}");

                        var versionDirs = Directory.GetDirectories(packageDir)
                            .Select(Path.GetFileName)
                            .Where(s => SemanticVersion.TryParse(s, out var _))
                            .Select(v => SemanticVersion.Parse(v))
                            .OrderByDescending(v => v)
                            .ToList();

                        if (versionDirs.Count <= 1)
                        {
                            Console.WriteLine("Нет старых версий для удаления.");
                            continue;
                        }

                        var latestVersion = versionDirs.First();

                        foreach (var version in versionDirs.Skip(1))
                        {
                            string versionPath = Path.Combine(packageDir, version.ToString());
                            try
                            {
                                Directory.Delete(versionPath, true);
                                Console.WriteLine($"Удалена версия: {version}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Не удалось удалить версию {version}: {ex.Message}");
                            }
                        }

                        Console.WriteLine($"Оставлена последняя версия: {latestVersion}");
                    }

                    Console.WriteLine("Очистка завершена.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }
    }
}
