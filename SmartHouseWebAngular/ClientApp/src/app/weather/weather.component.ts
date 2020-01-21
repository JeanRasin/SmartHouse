import { Component } from '@angular/core'

import { HttpService } from 'src/app/http.service'
import { Weather } from 'src/app/weather'

@Component({
  selector: 'weather-component',
  templateUrl: './weather.component.html',
  providers: [HttpService]
})
export class WeatherComponent {

  //public n: number = 150;
  //public items: string[] = [];
  public weatherData: Weather;
  public temp: number;

  constructor(private httpService: HttpService) { }

  ngOnInit() {

    this.httpService.getWeather().subscribe((data: Weather) => this.temp = data.Temp);

  }
}

