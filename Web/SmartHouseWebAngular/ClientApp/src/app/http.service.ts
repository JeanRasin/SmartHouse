import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

//import { Weather } from 'src/app/weather'

@Injectable()
export class HttpService {

 // url: string = 'http://localhost:5544/api';
   url: string = 'http://localhost:55673/api';

  constructor(private http: HttpClient) { }

  getWeather() {
    return this.http.get(`${this.url}/weather`);
  }

  getLogger() {
    return this.http.get(`${this.url}/logger`);
  }

  getGoal() {
    return this.http.get(`${this.url}/goal`);
  }

  deleteGoal(id: string) {
    return this.http.delete(`${this.url}/goal/${id}`);
  }

  checkGoal(id: string, done: boolean) {
  //  return this.http.put(`${this.url}/goal/done/${id}`, { id: id, done: done }); //Todo: verify
    return this.http.put(`${this.url}/goal/done/`, { id: id, done: done });
  }

  createGoal(name: string) {
    return this.http.post(`${this.url}/goal/`, { name: name });
  }

  editGoal(name: string) {
    return this.http.put(`${this.url}/goal/`, { name: name });
  }
}
