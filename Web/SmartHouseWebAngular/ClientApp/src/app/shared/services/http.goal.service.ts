import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Goal service.
 */
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
    return this.http.put(`${this.baseUrl}/api/goal/done`, { id: id, done: done });
  }

  create(name: string) {
    return this.http.post(`${this.baseUrl}/api/goal`, { name: name });
  }

  edit(id: string, name: string) {
    return this.http.put(`${this.baseUrl}/api/goal`, { id: id, name: name });
  }
}
