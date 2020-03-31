# 🏠 Smart House System 
**Smart Home System** - Проект реализации системы "умный дом". А так же для эксперементирования с разными технологиями.

## 🧱 Функции системы

## 🗜 Элементы системы
1. 🖥 Web Front сделан на [Angular](https://github.com/angular). Проект [SmartHouseWebAngular](Web/SmartHouseWebAngular).
2. ⚙️ API для фронта сделан на .[NET Core](https://github.com/dotnet/core). Проект [SmartHouseAPI](API/SmartHouseAPI).
3. 📘 Для основной информации хранения данных используется база данных [PostgreSql](https://github.com/postgres).
4. 📗 Для хранения логов используется база данных [MongoDB](https://github.com/mongodb).
5. 🌡️ Данные о погоде поступают из сервиса [Open Weather](https://openweathermap.org). Проект [SmartHouse.Service.Weather.OpenWeatherService](SmartHouse.Service.Weather.OpenWeatherService).

## 🔬 Тесты системы
1. 🧪 Для юнит тестирования фронта используется фреймворк [Karma](https://karma-runner.github.io/latest/index.html), а для интеграционного [Protractor](https://github.com/angular/protractor). Процесс запуска [тут](Web/SmartHouseWebAngular/README.md#-unit-тесты).
2. ⚗️ API тесты реализуют юнит и интеграционное тестирование на технологиях [NUnit](https://github.com/nunit) и [XUnit](https://github.com/xunit). Процесс запуска [тут](Tests/README.md).

## ⚙️ Эмуляция сервисов
Для эмуляции работы сервисов используется программа [SoapUI]() где [файлы](Other\SoapUI%20Services) для эмуляции работы API сервисов и сервиса погоды.
Так же для эмуляции сервиса погоды используется проект [WetherApiNodejs](SmartHouse.Service.Weather.OpenWeatherService) написанный на [Node.js](https://github.com/nodejs).

## 🐳 Docker 
Все функции проекта можно запустить в [Docker](https://github.com/docker) так же как и тесты.

Для запуска много контейнерного приложения необходимо выполнить инструкцию через консоль из корня решения. Файл для описания много контейнерного приложения [docker-compose.yml](https://github.com/JeanRasin/SmartHouse/blob/master/docker-compose.yml). 
```docker-compose
docker-compose -f docker-compose.yml up -d
```
Для запуска вместе с тестами необходимо выполнить команду в консоле. Файл для описания много контейнерного приложения [docker-compose.test.yml](https://github.com/JeanRasin/SmartHouse/blob/master/docker-compose.test.yml). 
```docker-compose
docker-compose -f docker-compose.yml -f docker-compose.test.yml up -d
```
Все порты которые использует проект находится в файле [.env](https://github.com/JeanRasin/SmartHouse/blob/master/.env).

## Авторство
 * **Rasin Jean** - Основная работа - [JeanRasin](https://github.com/JeanRasin)

