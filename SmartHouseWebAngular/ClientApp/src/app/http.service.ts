import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Weather } from 'src/app/weather'

@Injectable()
export class HttpService {

  url: string = 'http://localhost:5544/api';


  constructor(private http: HttpClient) { }

  getWeather() {
    return this.http.get(`${this.url}/weather`);
  }

  getLogger() {
    return this.http.get(`${this.url}/logger`);
  }
}
