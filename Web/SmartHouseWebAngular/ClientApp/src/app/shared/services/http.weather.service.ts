import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Weather } from '..';

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
  get() : Observable<Weather> {
    return this.http.get<Weather>(`${this.baseUrl}/api/weather`);
  }
}
