Yii Web Programming Framework
=============================

Thank you for choosing Yii - a high-performance component-based PHP framework.


INSTALLATION
------------

Please make sure the release file is unpacked under a Web-accessible
directory. You shall see the following files and directories:

      demos/               demos
      framework/           framework source files
      requirements/        requirement checker
      CHANGELOG            describing changes in every Yii release
      LICENSE              license of Yii
      README               this file
      UPGRADE              upgrading instructions


REQUIREMENTS
------------

The minimum requirement by Yii is that your Web server supports
PHP 5.1.0 or above. Yii has been tested with Apache HTTP server
on Windows and Linux operating systems.

Please access the following URL to check if your Web server reaches
the requirements by Yii, assuming "YiiPath" is where Yii is installed:

      http://hostname/YiiPath/requirements/index.php


QUICK START
-----------

Yii comes with a command line tool called "yiic" that can create
a skeleton Yii application for you to start with.

On command line, type in the following commands:

        $ cd YiiPath/framework                (Linux)
        cd YiiPath\framework                  (Windows)

        $ ./yiic webapp ../testdrive          (Linux)
        yiic webapp ..\testdrive              (Windows)

The new Yii application will be created at "YiiPath/testdrive".
You can access it with the following URL:

        http://hostname/YiiPath/testdrive/index.php


WHAT's NEXT
-----------

Please visit the project website for tutorials, class reference
and join discussions with other Yii users.



The Yii Developer Team
http://www.yiiframework.com







Пакеты
Ангуляр
[Angular CLI](https://github.com/angular/angular-cli) version 8.3.14.
Компоненты
@angular/material": "~8.2.2"

Запустить проект
npm start. Страница с проектом будет по адресу http://localhost:4200

Запустить тесты
npm test. Страница с юнит тестами откроется по адресу http://localhost:9876

Запустить интеграцонные тесты
npm e2e.


Docker
docker build -t shwa . - сделать образ.
docker run -p  4200:4200 -p 9876:9876 shwa - запустить образ. 4200 порт проекта. 9876 порт для тестов.

Mock API
Mock REST реализован с помощью программы SoapUI. Путь к проекту "/Other/SoapUI Services/SoapUI-HTTP-API.xml" Проект использует адрес API http://localhost:55673





protractor проект тестирования e2e
karma unit tests



# Smart House

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 8.3.14.
В качестве набора компонентов использовался [Angular Material UI](https://material.angular.io/) version 8.3.14.

## Development server

Run `ng serve` or `npm start` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).








