//https://blog.knoldus.com/unit-testing-of-angular-service-with-httpclient/

import { TestBed, inject } from '@angular/core/testing';
import { HttpLoggerService, Logger } from '../../app/shared';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';

export class LoggerServiceTest {
  static test() {
    describe('Http logger service test', () => {

      const dataLog: Logger[] = [
        {
          logLevel: 5,
          eventId: {
            stateId: 4,
            name: 'oxkpcdlvch'
          },
          message: 'Metal Unbranded Plastic Hat calculating architect Buckinghamshire Cayman Islands Dollar payment revolutionary Massachusetts HTTP USB Music Money Market Account Kansas sky blue Metical XML Handmade Frozen Fish open-source Ergonomic',
          date: '1997-01-07T19:13:17.4527845',
          id: 'db34c39380277502a9e4e94a9b7fba23'
        },
        {
          logLevel: 4,
          eventId: {
            stateId: 4,
            name: 'uxhdzwuefj'
          },
          message: 'Barbados Dollar neural Turkish Lira matrix Refined Cotton Towels Credit Card Account haptic Rustic Plastic Shoes Borders web-enabled Licensed Steel Fish multi-tasking transitional iterate indigo 24/7 Barbados Dollar Central Generic Plastic Pants withdrawal',
          date: '1997-01-06T12:58:32.0156938',
          id: '392f2789428110da31b60015c06aca4e'
        },
        {
          logLevel: 3,
          eventId: {
            stateId: 6,
            name: 'fsmagbkfoz'
          },
          message: 'bandwidth hacking Mountains Factors Virtual Buckinghamshire Industrial networks end-to-end haptic Games, Automotive \u0026 Books Ergonomic Metal Tuna array Handmade Frozen Mouse 1080p Wooden hard drive mesh matrix Ville',
          date: '1997-01-10T03:41:03.1171229',
          id: 'a649305f03a1407b2d1e7a0e619f7a71'
        }];

      beforeEach(async () => {
        TestBed.configureTestingModule({
          imports: [HttpClientTestingModule],
          providers: [HttpLoggerService]
        });
      });

      afterEach(inject([HttpTestingController], (httpMock: HttpTestingController) => {
        httpMock.verify();
      }));

      it('create logger service', inject([HttpTestingController, HttpLoggerService],
        (service: HttpLoggerService) => {
          service = TestBed.get(HttpLoggerService);
          expect(service).toBeTruthy();
        })
      );

      it('#get get array loggers', inject([HttpTestingController, HttpLoggerService],
        (httpMock: HttpTestingController, service: HttpLoggerService) => {
          service.get().subscribe(data => {
            expect(data).toEqual(dataLog);
          })

          const request = httpMock.expectOne(`${service.baseUrl}/api/logger`);
          expect(request.request.method).toBe('GET');
          request.flush(dataLog);
        })
      );

    });
  };
}