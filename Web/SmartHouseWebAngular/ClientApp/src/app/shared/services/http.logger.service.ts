import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

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
  get() {
    return this.http.get(`${this.baseUrl}/api/logger`);
  }
}
