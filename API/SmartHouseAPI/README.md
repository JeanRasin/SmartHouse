# ⚙️ REST API
**ASP.NET Core REST API** - Backend проект для реализации API системы умный дом. Реализовано в проекте [API\SmartHouseAPI\SmartHouseAPI.csproj](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/SmartHouseAPI.csproj). Исполльзует [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) автоматическую систему документирования API. Открыть можно по адресау `http://localhost:55673/api/docs/index.html` json описание `http://localhost:55673/api/docs/v1/swagger.json`.

## 🎮 Контроллеры
1. 🌡️ Контроллер погоды [WeatherController.cs](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/Controllers/WeatherController.cs).
2. 📗 Контроллер логов [LoggerController.cs](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/Controllers/LoggerController.cs).
3. 📋 Контроллер работы с задачами [GoalController.cs](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/Controllers/GoalController.cs).
4. ❗️ Контроллер ошибок [ErrorController.cs](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/Controllers/ErrorController.cs).

## ⚙️ Эмуляция
Для эмуляции работы API используется программа [SoapUI](https://github.com/SmartBear/soapui) где файл [SoapUI-HTTP-API.xml](https://github.com/JeanRasin/SmartHouse/blob/master/Other/SoapUI%20Services/SoapUI-HTTP-API.xml) для эмуляции работы контроллеров. Для работы контроллера получение погоды в режиме `debug` можно использовать проект [SoapUI-OpenWeatherMap.xml](https://github.com/JeanRasin/SmartHouse/blob/master/Other/SoapUI%20Services/SoapUI-OpenWeatherMap.xml).

## 📦 Пакеты
* [.NET Core](https://github.com/dotnet/core) (3.1) - модульная платформа для разработки программного обеспечения с открытым исходным кодом. Совместима с такими операционными системами как Windows, Linux и macOS.
* [Bogus](https://github.com/bchavez/Bogus) (29.0.1) - генератор поддельных данных.
* [Swashbuckle.AspNetCore.Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) (5.0.0) - инструменты для документирования API.
* [Microsoft.EntityFrameworkCore.Tools](https://github.com/dotnet/efcore) (16.2.0) - изменение object-database карты для .NET. Поддерживает запросы LINQ, отслеживание изменений, обновления и миграцию схем.
* [Npgsql.EntityFrameworkCore.PostgreSQL](https://github.com/npgsql/efcore.pg) (3.1.2) - Entity Framework Core провайдер для PostgreSQL.

## 👽 Авторство
 * **Rasin Jean** - Вся работа - [JeanRasin](https://github.com/JeanRasin)
 
## 📜 Лицензия
Этот проект лицензирован по лицензии MIT - подробности см. В файле [LICENSE](https://github.com/JeanRasin/SmartHouse/blob/master/LICENSE).
