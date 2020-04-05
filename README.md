# 🏠 Smart House System 
**Smart Home System** - Проект реализации системы "умный дом". А так же для экспериментирования с разными технологиями.

## 🧱 Функции системы
1. 🌩️ Механизм получения погоды из сервиса погоды.
2. 🗃 Сохранение и отображение логов системы.
3. 📋 Механизм работы с задачами. Опеации над задачами: отображение, создание, изменение, удаление и отметка что выполненно. 

## 🗜 Элементы системы
1. 🖥 Web Front сделан на [Angular](https://github.com/angular). Проект [SmartHouseWebAngular](Web/SmartHouseWebAngular). Подробнее [тут](https://github.com/JeanRasin/SmartHouse/blob/master/Web/SmartHouseWebAngular/README.md).
2. ⚙️ REST API для фронта сделан на .[NET Core](https://github.com/dotnet/core). Проект [SmartHouseAPI](API/SmartHouseAPI). Подробнее [тут](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI/README.md).
3. 📘 Для основной информации хранения данных используется база данных [PostgreSQL](https://github.com/postgres).
4. 📗 Для хранения логов используется база данных [MongoDB](https://github.com/mongodb).
5. 🌡️ Данные о погоде поступают из сервиса [Open Weather](https://openweathermap.org). Проект [SmartHouse.Service.Weather.OpenWeatherService](SmartHouse.Service.Weather.OpenWeatherService).

## 🏗️ Архитектура back-end приложения 
### 🔴 Application Core
В качестве арахитектуры проекта используется ["Onion Architecture"](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) ("луковая" архитектура).
1. **Domain Model** - классы моделей, которые используются в приложении и объекты которых хранятся в базе данных. Проект [SmartHouse.Domain.Core](https://github.com/JeanRasin/SmartHouse/tree/master/SmartHouse.Domain.Core).
2. **Domain Services** - уровень которые управляют работой с моделью домена в виде интерфейсов репозиториев. Проект [SmartHouse.Domain.Interfaces](https://github.com/JeanRasin/SmartHouse/tree/master/SmartHouse.Domain.Interfaces).
3. **Application Services** - уровень API или интерфейсов бизнес-логики приложения. Проект [SmartHouse.Services.Interfaces](https://github.com/JeanRasin/SmartHouse/tree/master/SmartHouse.Services.Interfaces).
### ⭕️ Infrastructure
1. **Repository** - Уровень инфроструктуры реализует интерфейсы, объявленные на нижних уровнях, и связывать их с хранилищем данных. Проект [SmartHouse.Infrastructure.Data](https://github.com/JeanRasin/SmartHouse/tree/master/SmartHouse.Infrastructure.Data).
2. **Business logic** - Реализация бизнес логики. Проект [SmartHouse.Infrastructure.Business](https://github.com/JeanRasin/SmartHouse/tree/master/SmartHouse.Infrastructure.Business).
3. **Web service** - Проекты получения данных погоды. Их может быть множество. Папка с проектами [Weather](https://github.com/JeanRasin/SmartHouse/tree/master/Weather).
4. **API** - Основной проект REST API back-end. Подробнее [тут](https://github.com/JeanRasin/SmartHouse/blob/master/API/SmartHouseAPI).
5. **Tests** - Тесты всех систем включающие юнит тесты и интеграционные тесты. Подробнее [тут](https://github.com/JeanRasin/SmartHouse/blob/master/Tests).

## 🖥 Архитектура front-end приложения
Система может поддерживать множество представлений, в данном случае используется одно представление написанное на [Angular](https://github.com/angular). Папка с проектами представлений [тут](https://github.com/JeanRasin/SmartHouse/tree/master/Web). Подробнее [тут](https://github.com/JeanRasin/SmartHouse/blob/master/Web/SmartHouseWebAngular/README.md).

## 🔬 Тесты системы
1. 🧪 Для юнит тестирования фронта используется фреймворк [Karma](https://karma-runner.github.io/latest/index.html), а для интеграционного [Protractor](https://github.com/angular/protractor). Процесс запуска [тут](Web/SmartHouseWebAngular/README.md#-unit-тесты).
2. ⚗️ API тесты реализуют юнит и интеграционное тестирование на технологиях [NUnit](https://github.com/nunit) и [XUnit](https://github.com/xunit). Процесс запуска [тут](Tests/README.md).

## ⚙️ Эмуляция сервисов
Для эмуляции работы сервисов используется программа [SoapUI](https://github.com/SmartBear/soapui) где [файлы](Other\SoapUI%20Services) для эмуляции работы API контроллеров [SoapUI-HTTP-API.xml](https://github.com/JeanRasin/SmartHouse/blob/master/Other/SoapUI%20Services/SoapUI-HTTP-API.xml) и сервиса погоды [SoapUI-OpenWeatherMap.xml](https://github.com/JeanRasin/SmartHouse/blob/master/Other/SoapUI%20Services/SoapUI-OpenWeatherMap.xml).
Так же для эмуляции сервиса погоды используется проект [WetherApiNodejs](SmartHouse.Service.Weather.OpenWeatherService) написанный на [Node.js](https://github.com/nodejs), который можно запустить в Docker файлом [docker-compose.yml](https://github.com/JeanRasin/SmartHouse/blob/376234a9d1989daf52081bc7b44f5e1726b11e9b/docker-compose.yml#L57).

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

## 🔮 Будущая разработка !!!

## 👽 Авторство
 * **Rasin Jean** - Вся работа - [JeanRasin](https://github.com/JeanRasin)
 
## 📜 Лицензия
Этот проект лицензирован по лицензии MIT - подробности см. В файле [LICENSE](https://github.com/JeanRasin/SmartHouse/blob/master/LICENSE).

