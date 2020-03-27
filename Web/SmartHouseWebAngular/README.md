# 🖥 Smart House Angular 

Этот проект фронта написан на [Angular CLI](https://github.com/angular/angular-cli) версии 8.3.14.
Который использует [TypeScript](https://github.com/microsoft/TypeScript) версии 3.5.3.
В качестве набора компонентов использовался [Angular Material UI](https://material.angular.io/) версии 8.3.14.

## 🚀 Запуск проекта
Убедитесь что у вас установлен [Node.js](https://nodejs.org/en/download). Перед запуском необходимо установить пакеты вводя команду в консоль `npm install` из папки [ClientApp](ClientApp). Так же нужно построить и проверить проект командой `npm run build`. Для запуска проекта необхоимо выполнить команду `npm start`. Проект откроется по адресу `http://localhost:4200/`. 

## 🧪 Unit тесты
Для запуска unit тестирования необходимо выполнить `npm run test` в качестве тестового фреймворка используется [Karma](https://karma-runner.github.io) версии 4.4.1. Для работы с тестами через браузер необъодимо перейти на страницу `http://localhost:9876/`.

## ⚗️ End-to-end тесты
Для запуска e2e тестирования необходимо выполнить `npm run e2e` в качестве тестового фреймворка используется [Protractor](http://www.protractortest.org/) версии 5.4.2 и [Jasmine](https://jasmine.github.io/) версии 3.5.7.

## 🐳 Docker 
Для запуска теста не обходимо в папке [проекта](https://github.com/JeanRasin/SmartHouse) запустить docker файл [Dockerfile](https://github.com/JeanRasin/SmartHouse/blob/master/Dockerfile) введя в консоле команду для сборки образа.
```docker
docker build --rm -t smart-house-web-angular-test .
```
Что бы запустить контейнер по образу необходимо выполнить команду.
```docker
docker run --name smart-house-web-angular-test_1 -p 9876:9876 -d smart-house-web-angular-test
```
Для запуска unit тестов необходимо ввести команду после чего можно открыть веб пердставление по адресу `http://localhost:9876/`.
```docker
 docker exec -it smart-house-web-angular-test_1 bash ng test
```
Для запуска e2e тестов необходимо ввести команду.
```docker
 docker exec -it smart-house-web-angular-test_1 bash ng e2e
```

