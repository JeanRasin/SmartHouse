import { Component } from '@angular/core'

import { HttpService } from 'src/app/http.service'
import { Weather } from 'src/app/weather'

@Component({
  selector: 'weather-component',
  templateUrl: './weather.component.html',
  providers: [HttpService]
})
export class WeatherComponent {

  weatherData: Weather;
  error: any = null;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.httpService.getWeather().subscribe((data: Weather) => {
      console.log(data);
      this.weatherData = data;
    }, error => {
      this.error = error.message;
      console.log(error);
    });
  }
}

