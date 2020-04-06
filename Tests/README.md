# 🔬 Тесты
Для API тестирования используются технологии [NUnit](https://github.com/nunit) и [XUnit](https://github.com/xunit/xunit). Вполне достаточно было использовать [XUnit](https://github.com/xunit/xunit), но для сравнения разных тестов было принето решение часть кода тестировать NUnut. В качестве архитектуры системы используется ["Onion Architecture"](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) состоящая из слоёв. Каждый слой отдельно тестируется.

## 🗜 Тестируемые системы
1. **Repository tests** - [Тесты](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestRepository) работы с хранилищем данных.
2. **Business tests** - [Тесты](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestBusiness) бизнес логики.
3. **Services tests** - [Тесты](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestServices) сервисов погоды.
4. **Api tests** - [Тесты](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestApi) back-end REST API.
5. **ApiIntegration tests** - Интеграционные [тесты](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestApiIntegration)  back-end REST API.

## 🚀 Запуск тестов
### 🧪 Unit тесты
Основные юнит тесты можно запустить через механизм тестирования [Visual Studio](https://visualstudio.github.com/). Либо через консоль из корня проекта теста используя команды 
`dotnet restore` для построения проекта и `dotnet test` для запуска теста. Например [тест](TestRepository) репозитория. Для запуска всех тестов необходиомо перейти в корень [решения](https://github.com/JeanRasin/SmartHouse) и запустить команду `dotnet test SmartHouse.sln`. Так же можно использовать команду `dotnet vstest <path\*.dll>...` для запуска отдельных тестов, (пример `dotnet vstest Tests\TestRepository\bin\Debug\netcoreapp3.1\RepositoryTest.dll`).
Все тесты используют фреймворк [XUnit](https://github.com/xunit/xunit) кроме [ServicesTest.csproj](https://github.com/JeanRasin/SmartHouse/blob/master/Tests/TestServices/ServicesTest.csproj) который использует [NUnit](https://github.com/nunit).

### ⚗️ Integration тесты
Интеграционный тест API используют фреймворк [XUnit](https://github.com/xunit/xunit) и использует действующий web сервер. По этому лучше использовать сервер который запускается в докер контейнере и который использует базы данных и сервисы которые то же запускаются на докере. Интеграционные тесты так же запускаются, как и юнит  тесты`dotnet vstest Tests\TestApiIntegration\bin\Debug\netcoreapp3.1\ApiIntegrationTest.dll` либо из [папки](https://github.com/JeanRasin/SmartHouse/tree/master/Tests/TestApiIntegration) теста ввести `dotnet test`.

## 🐳 Docker
В докере так же можно запустить все тесты. Юнит тесты могут исполнятся в одном контейнере, а вот для интеграционных нужно запускать группу связанных контейнеров для развертывания сервиса API, баз данных и сервиса погоды. Для запуска все тестов необходимо выполнить команду из [корня](https://github.com/JeanRasin/SmartHouse) решения.
```docker-compose
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d
```
Юнит и интеграционные тесты выполняются в разных контейнерах. Результаты тестов можно будет увидеть в консоле контейнеров. После выполения работы контейнеры выключаются.
Для построения образа тестов используется файл [Dockerfile.tests](https://github.com/JeanRasin/SmartHouse/blob/master/Dockerfile.tests) с его помошью можно запускать отдельные тесты.
Для начала нужно собрать образ из [корня](https://github.com/JeanRasin/SmartHouse) решения.
```docker
docker build -f Dockerfile.tests --rm -t all-test .
```
После запустить контейнер.
```docker
docker run --name all-test_business -d all-test
```
И запустить тест.
```docker
docker exec -it all-test_business bash dotnet vstest TestBusiness/BusinessTest.dll
```
Интеграционный тест запускается так же, но нужно запустить вспомогательные контейнеры командой, причем уже без `docker-compose.test.yml` файла тестов.
```docker-compose
docker-compose -f docker-compose.yml up -d
```
Но и это еще не все. Нашему интеграционному тесту контейнеру нужно дать доступ к сети остальных контейнеров запустив его командой.
```docker
docker run -it --name all-test_integration --network smarthouse_smart-house-network -d all-test
```
 И запустить тест.
```docker
docker exec -it all-test_business bash dotnet vstest TestApiIntegration/ApiIntegrationTest.dll
```
## 📦 Пакеты
* [XUnit](https://github.com/xunit/xunit) (2.4.1) - Инструмент для модульного тестирования с открытым исходным кодом для .NET.
* [NUnit](https://github.com/nunit) (3.12.0) - Открытая среда юнит-тестирования приложений для .NET.
* [Moq](https://github.com/moq/moq4) (4.13.1) - Библиотека моделирования объектов, которые эммитируют поведение реальных объектов контролируемыми способами.
* [EntityFrameworkCore.Testing.Moq](https://github.com/rgvlee/EntityFrameworkCore.Testing) (2.2.0) - Библиотека моделирования объектов, которые эммитируют поведение [Entity Framework Core](https://github.com/dotnet/efcore).
* [Bogus](https://github.com/bchavez/Bogus) (29.0.1) - Генератор поддельных данных.
* [Microsoft.NET.Test.Sdk](https://github.com/microsoft/vstest) (16.5.0) - Средство запуска и механизм, который обеспечивает работу тестового обозревателя и vstest.console.

### Integration
* [Microsoft.AspNetCore.TestHost](https://github.com/aspnet/Hosting/tree/master/src/Microsoft.AspNetCore.TestHost) (3.1.3) - веб-сервер ASP.NET Core для написания и запуска тестов.

## 👽 Авторство
 * **Rasin Jean** - Вся работа - [JeanRasin](https://github.com/JeanRasin)
 
## 📜 Лицензия
Этот проект лицензирован по лицензии MIT - подробности см. В файле [LICENSE](https://github.com/JeanRasin/SmartHouse/blob/master/LICENSE).
