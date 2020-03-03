import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Goal } from '..';

/**
 * Goal service.
 */
@Injectable()
export class HttpGoalService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public get(): Observable<Goal[]> {
    return this.http.get<Goal[]>(`${this.baseUrl}/api/goal`);
  }

  public getAll(): Observable<Goal[]> {
    return this.http.get<Goal[]>(`${this.baseUrl}/api/goal/getAll`);
  }

  public delete(id: string): Observable<object> {
    return this.http.delete<object>(`${this.baseUrl}/api/goal/${id}`);
  }

  public check(id: string, done: boolean): Observable<object> {
    return this.http.put<object>(`${this.baseUrl}/api/goal/done`, { id: id, done: done });
  }

  public create(name: string): Observable<Goal> {
    return this.http.post<Goal>(`${this.baseUrl}/api/goal`, { name: name });
  }

  public edit(id: string, name: string): Observable<object> {
    return this.http.put<object>(`${this.baseUrl}/api/goal`, { id: id, name: name });
  }
}
