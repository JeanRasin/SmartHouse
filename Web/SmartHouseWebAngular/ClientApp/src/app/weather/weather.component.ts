import { Component, OnInit } from '@angular/core';
import { Weather } from '../shared';
import { HttpWeatherService } from '../shared/services';

@Component({
  selector: 'app-weather-component',
  templateUrl: './weather.component.html',
  providers: [HttpWeatherService]
})
export class WeatherComponent implements OnInit {
  weatherData: Weather = new Weather();
  error: any = null;

  constructor(private httpService: HttpWeatherService) { }

  ngOnInit() {
    const result = this.httpService.get()

    result.subscribe((data: Weather) => {
      this.weatherData = data;
      console.log(this.weatherData);
    }, error => {
      this.error = error.message;
    });
  }
}

