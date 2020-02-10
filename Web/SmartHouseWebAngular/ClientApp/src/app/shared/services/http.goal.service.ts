import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable()
export class HttpGoalService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }
  get() {
    return this.http.get(`${this.baseUrl}/api/goal`);
  }
  getAll() {
    return this.http.get(`${this.baseUrl}/api/goal/getAll`);
  }
  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/api/goal/${id}`);
  }
  check(id: string, done: boolean) {
    //  return this.http.put(`${this.url}/goal/done/${id}`, { id: id, done: done }); //Todo: verify
    return this.http.put(`${this.baseUrl}/api/api/goal/done/`, { id: id, done: done });
  }
  create(name: string) {
    return this.http.post(`${this.baseUrl}/api/goal/`, { name: name });
  }
  edit(name: string) {
    return this.http.put(`${this.baseUrl}/api/goal/`, { name: name });
  }
}
