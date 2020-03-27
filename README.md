# SmartHouse
Smart home system.


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