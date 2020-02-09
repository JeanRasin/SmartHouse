import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { GoalService } from './shared/services';

@Injectable()
export class HttpService {

  // url: string = 'http://localhost:5544/api';
  public url: string = 'http://localhost:55673/api';
  public goal: GoalService;

  constructor(private http: HttpClient) {
    this.goal = new GoalService(http, this.url);
  }

  getWeather() {
    return this.http.get(`${this.url}/weather`);
  }

  getLogger() {
    return this.http.get(`${this.url}/logger`);
  }


}
