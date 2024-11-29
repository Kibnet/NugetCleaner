using NuGet.Versioning;
using NuGet.Configuration;

namespace NugetCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            if (!osNameAndVersion.Contains("Windows"))
            {
                Console.WriteLine("Этот скрипт работает только на Windows!");
                return;
            }

            var settings = Settings.LoadDefaultSettings(".");
            var rootDirectory = SettingsUtility.GetGlobalPackagesFolder(settings);

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
                        .Where(s => s != null && SemanticVersion.TryParse(s, out _))
                        .Select(SemanticVersion.Parse!)
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
