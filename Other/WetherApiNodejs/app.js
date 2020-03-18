// подключение express.
const express = require("express");

// создаем объект приложения.
const app = express();

// определяем обработчик для маршрута "/".
app.get("/", function (request, response) {

    // отправляем ответ.
    response.send("<h2>Hello Weather REST API</h2>");
});

app.get("/data/2.5/*", function (request, response) {

    var weatherJson = `{
   "coord":{
      "lon":56.29,
      "lat":58.02
   },
   "weather":[
      {
         "id":600,
         "main":"Snow",
         "description":"light snow",
         "icon":"13n"
      },
      {
         "id":701,
         "main":"Mist",
         "description":"mist",
         "icon":"50n"
      }
   ],
   "base":"stations",
   "main":{
      "temp":-1,
      "feels_like":-5.23,
      "temp_min":-1,
      "temp_max":-1,
      "pressure":977,
      "humidity":100
   },
   "visibility":4700,
   "wind":{
      "speed":300,
      "deg":290
   },
   "clouds":{
      "all":75
   },
   "dt":1579718700,
   "sys":{
      "type":1,
      "id":8984,
      "country":"RU",
      "sunrise":1579667945,
      "sunset":1579695178
   },
   "timezone":18000,
   "id":511196,
   "name":"Perm",
   "cod":200
}`;

    // отправляем ответ.
    response.setHeader('Content-Type', 'application/json');
    response.send(weatherJson);
});

// начинаем прослушивать подключения на 5412 порту.
app.listen(5412);