# 🏠 Smart House System 
Smart Home System. - Проект для системы домашней интеллектуальной системы. А так же для эксперементирования с разными технологиями.

## Система состоит из элементов:
1. 🕸 Web Front сделан на [Angular](https://github.com/angular). Проект [SmartHouseWebAngular](Web/SmartHouseWebAngular).
2. ⚙️ API для фронта сделан на .[NET Core](https://github.com/dotnet/core). Проект [SmartHouseAPI](API/SmartHouseAPI).
3. 📘 Для основной информации хранения данных используется база данных [PostgreSql](https://github.com/postgres).
4. 📗 Для хранения логов используется база данных [MongoDB](https://github.com/mongodb).
5. 🌡️ Данные о погоде поступают с сервиса [Open Weather](https://openweathermap.org). Проект [SmartHouse.Service.Weather.OpenWeatherService](SmartHouse.Service.Weather.OpenWeatherService).

## Для всех элементов реализованны тесты:
1. 🧪 Web Front использует тесты. Для юнит тестирования используется фреймворк [Karma](https://karma-runner.github.io/latest/index.html), а для интеграционного [Jasmine](https://jasmine.github.io). [Процесс запуска](Web/SmartHouseWebAngular/README.md).
2. ⚗️ API тесты реализуют юнит и интеграционное тестирование на технологиях [NUnit](https://github.com/nunit) и [XUnit](https://github.com/xunit). [Процесс запуска](Tests/README.md).

## Эмуляция сервисов
Для эмуляции работы сервисов используется программа [SoapUI](). [Файлы](Other\SoapUI Services) для запуска.


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
