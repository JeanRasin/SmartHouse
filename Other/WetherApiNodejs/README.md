# 🌡️ Эмуляция сервиса погоды
Получить погоду по адресу `http://localhost:5412/data/2.5/`.
Сделать образ `docker build -t weather-api-nodejs .`, запустить контейнер `docker run -it --name weather-api-nodejs -p 5412:5412 -d weather-api-nodejs`