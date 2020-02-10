import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Wether service.
 */
@Injectable()
export class HttpWeatherService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  /**
   * Get weather.
   */
  get() {
    return this.http.get(`${this.baseUrl}/api/weather`);
  }
}
