import { Component } from '@angular/core'
import { Weather } from '../shared';
import { HttpWeatherService } from '../shared/services';

@Component({
  selector: 'weather-component',
  templateUrl: './weather.component.html',
  providers: [HttpWeatherService]
})
export class WeatherComponent {

  weatherData: Weather;
  error: any = null;

  constructor(private httpService: HttpWeatherService) { }

  ngOnInit() {
    this.httpService.get().subscribe((data: Weather) => {
      //console.log(data);
      this.weatherData = data;
    }, error => {
      this.error = error.message;
      //console.log(error);
    });
  }
}

