//https://blog.knoldus.com/unit-testing-of-angular-service-with-httpclient/

import { TestBed, inject } from '@angular/core/testing';
import { HttpWeatherService, Weather } from '../../app/shared';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';

export class WeatherServiceTest {
  static test() {
    describe('Http wetaher service test', () => {
      const dataWeather: Weather = {
        WindSpeed: 300,
        WindDeg: 290,
        Temp: -1,
        City: 'Perm',
        Pressure: 977,
        Humidity: 100,
        Description: 'light snow; mist',
        FeelsLike: -5.23
      };

      beforeEach(async () => {
        TestBed.configureTestingModule({
          imports: [HttpClientTestingModule],
          providers: [HttpWeatherService]
        });
      });

      afterEach(inject([HttpTestingController], (httpMock: HttpTestingController) => {
        httpMock.verify();
      }));

      it('create wether service', () => {
        const service: HttpWeatherService = TestBed.get(HttpWeatherService);
        expect(service).toBeTruthy();
      });

      it('#get get weather', inject([HttpTestingController, HttpWeatherService],
        (httpMock: HttpTestingController, service: HttpWeatherService) => {
          service.get().subscribe(data => {
            expect(data).toEqual(dataWeather);
          });

          const request = httpMock.expectOne(`${service.baseUrl}/api/weather`);
          expect(request.request.method).toBe('GET');
          request.flush(dataWeather);
        })
      );

    });
  };
}