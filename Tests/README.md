# 🧪 API тесты
Для API тестирования используются технологии NUnit и XUnit. В полне достаточно было использовать XUnit, но для сравнения разных тестов было принето решение часть кода тестировать NUnut.
API состоит из вертикально зависимых слоёв (Repository, Business logic, Net. Core API и Service) и каждый слой тестируется отдель и независимо.

## 🧪 Unit тесты
Основные юнит тесты можно запустить через механизм тестирования Visual Studio. Либо через консоль из корня проекта теста используя команды 
`dotnet restore` для построения проекта и `dotnet test` для запуска теста. Например [тест](TestRepository) репозитория. Для запуска всех тестов необходиомо перейти в корень [решения](https://github.com/JeanRasin/SmartHouse) и запустить команду `dotnet test SmartHouse.sln`. Так же можно использовать команду `dotnet vstest <path\*.dll>...` для запуска отдельных тестов, например `dotnet vstest Tests\TestRepository\bin\Debug\netcoreapp3.1\RepositoryTest.dll`.
Все тесты используют фреймворк XUnit кроме [ServicesTest.csproj](https://github.com/JeanRasin/SmartHouse/blob/master/Tests/TestServices/ServicesTest.csproj) который использует NUnit.

## ⚗️ Integration тесты
Интеграционный тест API используют фреймворк XUnit и использует действуюший web сервер. По этому лучше использовать сервер который запускается в докер контейнере и который использует базы данных и сервисы которые то же запускаются на докере. Интеграционые тесты так же запускаются, как и юнит `dotnet vstest Tests\TestApiIntegration\bin\Debug\netcoreapp3.1\ApiIntegrationTest.dll`. 

## 🐳 Docker
