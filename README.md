# SmartHouse
Smart Home System. - Проект для системы домашней интеллектуальной системы. А так же для эксперементирования с разными технологиями.
Система состоит из элементов:
1. Web Front сделан на Angular. [SmartHouseWebAngular](Web/SmartHouseWebAngular).
2. API для фронта сделан на .NET Core. [SmartHouseAPI](API/SmartHouseAPI).
3. Для осноыной информации хранения данных используется база данных PostgreSql.
4. Для хранения логов используется база данных MongoDB

Для всех элементов реализованны тесты:
1. Web Front использует два вида тестов. Для юнит тестирования используется фреймворк Karma, а для интеграционного Jasmine. [Процесс запуска](Web/SmartHouseWebAngular/README.md).
2. API тесты реализуют юнит и интеграционное тестирование на технологиях NUnit и XUnit. [Процесс запуска](Tests/README.md).

Построить образ с тестами.
docker build -f DockerfileTests -t tests-smart-house . 

Запустить все тесты в консоле
docker run -it --rm --name tests-smart-house tests-smart-house dotnet vstest TestRepository/RepositoryTest.dll TestBusiness/BusinessTest.dll TestApi/ApiTest.dll TestApiIntegration/ApiIntegrationTest.dll TestServices/ServicesTest.dll


Запустить многоконтейнерное приложение
docker-compose -f docker-compose.yml up -d

Запустить многоконтейнерное приложение и тесты
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d

Запустить только тесты
docker-compose -f docker-compose.test.yml up -d
