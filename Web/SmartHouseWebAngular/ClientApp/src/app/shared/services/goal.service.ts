import { HttpClient } from '@angular/common/http';

export class GoalService {
  constructor(private http: HttpClient, private url: string) { }
  get() {
    return this.http.get(`${this.url}/goal`);
  }
  getAll() {
    return this.http.get(`${this.url}/goal/getAll`);
  }
  delete(id: string) {
    return this.http.delete(`${this.url}/goal/${id}`);
  }
  check(id: string, done: boolean) {
    //  return this.http.put(`${this.url}/goal/done/${id}`, { id: id, done: done }); //Todo: verify
    return this.http.put(`${this.url}/goal/done/`, { id: id, done: done });
  }
  create(name: string) {
    return this.http.post(`${this.url}/goal/`, { name: name });
  }
  edit(name: string) {
    return this.http.put(`${this.url}/goal/`, { name: name });
  }
}
