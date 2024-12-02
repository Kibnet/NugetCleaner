# NugetCleaner

Это утилита для очистки старых версий пакетов в каталоге, работающая на основе семантического версионирования.

## Установка

Установить утилиту можно с помощью `dotnet`:

```bash
dotnet tool install --global CleanOldNuget
dotnet tool install --global CleanOldNuget --add-source https://nuget.pkg.github.com/kibnet/index.json
```

## Использование

В командной строке вызывать:
```bash
clean-old-nuget
```
