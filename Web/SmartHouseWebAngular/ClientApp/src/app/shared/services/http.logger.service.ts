import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Logger } from '..';

/**
 * Log service.
 */
@Injectable()
export class HttpLoggerService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  /**
   * Get logs.
   */
  public get(): Observable<Logger[]> {
    return this.http.get<Logger[]>(`${this.baseUrl}/api/logger`);
  }
}
