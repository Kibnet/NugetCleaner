![Logo](NugetCleaner/logo128.png)
# NugetCleaner

Это утилита для очистки старых версий Nuget-пакетов из кэша, работающая на основе семантического версионирования.
Утилита оставляет только последнюю версию каждого пакета, остальные удаляет.
Может освободить несколько десятков гигабайт на диске при первом использовании.

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
