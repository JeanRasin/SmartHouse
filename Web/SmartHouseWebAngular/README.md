# 🖥 WEB Angular 
Веб представление проекта написанный на платформе [Angular CLI](https://github.com/angular/angular-cli). Проект содержит главную страницу `http://localhost:4200` с компонентом погоды. На странице погоды `http://localhost:4200/weather` содержится отображение погоды. На странице логов `http://localhost:4200/log` выводятся все логи [backend](https://github.com/JeanRasin/SmartHouse/tree/master/API/SmartHouseAPI) проекта.
На странице `http://localhost:4200/goal` выводится список с задачами.

## 🚀 Запуск проекта
Убедитесь что у вас установлен [Node.js](https://nodejs.org/en/download). Перед запуском необходимо установить пакеты вводя команду в консоль `npm install` из папки [ClientApp](ClientApp). Так же нужно построить и проверить проект командой `npm run build`. Для запуска проекта необхоимо выполнить команду `npm start`. Проект откроется по адресу `http://localhost:4200`. Предварительно необходимо запустить [API](https://github.com/JeanRasin/SmartHouse/tree/master/API/SmartHouseAPI) проекта, заглушку [SoapUI Service](https://github.com/JeanRasin/SmartHouse/tree/master/Other/SoapUI%20Services) или docker [сервис](https://github.com/JeanRasin/SmartHouse/blob/6cdb2ed65d9bc32ec7227485b7161026adab780a/docker-compose.yml#L17).

## 🔬 Тесты
### 🧪 Unit тесты
Для запуска Unit тестирования необходимо выполнить `npm run test` в качестве тестового фреймворка используется [Karma](https://karma-runner.github.io). Для работы с тестами через браузер необходимо перейти на страницу `http://localhost:9876`.

### ⚗️ End-to-end тесты
Для запуска e2e тестирования необходимо выполнить `npm run e2e` в качестве тестового фреймворка используется [Protractor](http://www.protractortest.org/) и [Jasmine](https://jasmine.github.io/).

## 🐳 Docker 
Для запуска теста не обходимо в папке [проекта](https://github.com/JeanRasin/SmartHouse) запустить docker файл [Dockerfile](https://github.com/JeanRasin/SmartHouse/blob/master/Dockerfile) введя в консоль команду для сборки образа.
```docker
docker build --rm -t smart-house-web-angular-test .
```
Что бы запустить контейнер по образу необходимо выполнить команду.
```docker
docker run --name smart-house-web-angular-test_1 -p 9876:9876 -d smart-house-web-angular-test
```
Для запуска unit тестов необходимо ввести команду после чего можно открыть веб пердставление по адресу `http://localhost:9876`.
```docker
 docker exec -it smart-house-web-angular-test_1 bash ng test
```
Для запуска e2e тестов необходимо ввести команду.
```docker
 docker exec -it smart-house-web-angular-test_1 bash ng e2e
```
И конечно проект и все его тесты можно запустить через [docker-compos](https://github.com/JeanRasin/SmartHouse), как это сделать можно прочитать [тут](https://github.com/JeanRasin/SmartHouse/blob/master/README.md#-docker). 

## 📦 Пакеты
### 🔩 Софт
* [Angular CLI](https://github.com/angular/angular-cli) (8.3.14) - Открытая и свободная платформа для разработки веб-приложений, написанная на языке TypeScript.
* [TypeScript](https://github.com/microsoft/TypeScript) (3.5.3) -  Язык программирования позиционируемый, как средство разработки веб-приложений, расширяющее возможности JavaScript.

### 🖼 UI Components
* [Angular Material UI](https://material.angular.io) (8.3.14) - Компоненты пользовательского интерфейса для мобильных и настольных приложений Angular..

### 🧪 Unit тесты
* [Karma](https://karma-runner.github.io) (4.4.1) - Инструмент, который позволяет выполнять код JavaScript в нескольких реальных браузерах.
* [Karma-jasmine](https://github.com/karma-runner/karma-jasmine) (2.0.1) - Плагин Karma - адаптер для платформы тестирования [Jasmine](https://github.com/jasmine/jasmine).

### ⚗️ End-to-end тесты
* [Protractor](https://github.com/angular/protractor) (5.4.2) - Комплексная E2E тестовая среда для приложений Angular и AngularJS.

## 👽 Авторство
 * **Rasin Jean** - Вся работа - [JeanRasin](https://github.com/JeanRasin)
 
## 📜 Лицензия
Этот проект лицензирован по лицензии MIT - подробности см. В файле [LICENSE](https://github.com/JeanRasin/SmartHouse/blob/master/LICENSE).
