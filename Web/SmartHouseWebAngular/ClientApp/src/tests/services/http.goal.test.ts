// https://blog.knoldus.com/unit-testing-of-angular-service-with-httpclient/

import { TestBed, inject } from '@angular/core/testing';
import { HttpGoalService, Goal } from '../../app/shared';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';

export class GoalServiceTest {
  static test() {
    const responseOk200 = { status: 200, statusText: 'OK' };
    const responseNotFound404 = { status: 404, statusText: 'Not Found' };

    describe('Http goal service tests', () => {

      const dataLog: Goal[] = [
        {
          id: 'c55940df-c000-c450-1601-5e9aa3ed8bbc',
          name: 'driver Officer Holy See (Vatican City State)',
          dateCreate: new Date('1997-01-13T02:08:14.756456'),
          dateUpdate: new Date('1997-01-29T18:59:41.684245'),
          done: false
        },
        {
          id: '38874b7d-ef81-4895-b86f-f718c4a81544',
          name: 'Plastic morph Balanced',
          dateCreate: new Date('1997-01-15T21:54:29.920099'),
          dateUpdate: new Date('1997-01-28T22:29:44.035771'),
          done: false
        },
        {
          id: '7dc7631d-3f1e-f8bb-166c-63b52a05db21',
          name: 'Ports Rupiah Awesome',
          dateCreate: new Date('1997-01-13T23:07:22.848384'),
          dateUpdate: new Date('1997-01-27T16:35:23.823537'),
          done: false
        },
        {
          id: '0037c991-eb6d-2819-491c-221a1c9c01a8',
          name: 'matrix Stravenue Usability',
          dateCreate: new Date('1997-01-06T11:03:38.286677'),
          dateUpdate: new Date('1997-01-26T17:58:46.010328'),
          done: false
        },
        {
          id: '735f337f-755a-4db6-19ef-a092f0c82c8c',
          name: 'Trail Handcrafted Granite Chicken Shores',
          dateCreate: new Date('1997-01-16T00:30:24.257496'),
          dateUpdate: new Date('1997-01-26T10:41:03.225911'),
          done: false
        }];

      beforeEach(async () => {
        TestBed.configureTestingModule({
          imports: [HttpClientTestingModule],
          providers: [HttpGoalService]
        });
      });

      afterEach(inject([HttpTestingController], (httpMock: HttpTestingController) => {
        httpMock.verify();
      }));

      it('#create goal service', () => {

        // Arrange
        const service: HttpGoalService = TestBed.get(HttpGoalService);

        // Assert
        expect(service).toBeTruthy();
      });

      it('#get get array goals', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Act
          service.get().subscribe(data => {

            // Assert
            expect(data).toEqual(dataLog);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal`);
          expect(request.request.method).toBe('GET');
          request.flush(dataLog, responseOk200);
        })
      );

      it('#getAll get all array goals', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Act
          service.getAll().subscribe(data => {

            // Assert
            expect(data).toEqual(dataLog);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal/getAll`);
          expect(request.request.method).toBe('GET');
          request.flush(dataLog, responseOk200);
        })
      );

      it('#delete delete goal', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '0037c991-eb6d-2819-491c-221a1c9c01a8';

          // Act
          service.delete(id).subscribe(data => {

            // Assert
            expect(data).toBeTruthy();
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal/${id}`);
          expect(request.request.method).toBe('DELETE');
          request.flush({}, responseOk200);
        })
      );

      it('#delete delete goal not found id', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '0037c991-eb6d-2819-491c-221a1c9c01a8';

          // Act
          service.delete(id).subscribe(data => {

            // Assert
            expect(data).toBeTruthy();
          }, error => {

            // Assert
            expect(error.status).toEqual(404);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal/${id}`);
          expect(request.request.method).toBe('DELETE');
          request.flush({}, responseNotFound404);
        })
      );

      it('#done check goal', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '0037c991-eb6d-2819-491c-221a1c9c01a8';
          const done = true;

          // Act
          service.check(id, done).subscribe(data => {

            // Assert
            expect(data).toEqual({});
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal/done`);
          expect(request.request.method).toBe('PUT');
          request.flush({}, responseOk200);
        })
      );

      it('#done check goal not found id', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '0037c991-eb6d-2819-491c-221a1c9c01a0';
          const done = true;

          // Act
          service.check(id, done).subscribe(data => {

            // Assert
            expect(data).toEqual({});
          }, error => {

            // Assert
            expect(error.status).toEqual(404);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal/done`);
          expect(request.request.method).toBe('PUT');
          request.flush({}, responseNotFound404);
        })
      );

      it('#create create goal', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const name = 'test name';

          const result: Goal = {
            id: '735f337f-755a-4db6-19ef-a092f0c82c8c',
            name: 'Trail Handcrafted Granite Chicken Shores',
            dateCreate: new Date('1997-01-16T00:30:24.257496'),
            dateUpdate: new Date('1997-01-26T10:41:03.225911'),
            done: false
          };

          // Act
          service.create(name).subscribe(data => {

            // Assert
            expect(data).toBeTruthy();
            expect(data).toEqual(result);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal`);
          expect(request.request.method).toBe('POST');
          request.flush(result, responseOk200);
        })
      );

      it('#edit edit goal', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '735f337f-755a-4db6-19ef-a092f0c82c8c';
          const name = 'test name';
          const done = true;

          // Act
          service.edit(id, name, done).subscribe(data => {

            // Assert
            expect(data).toEqual({});
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal`);
          expect(request.request.method).toBe('PUT');
          request.flush({}, responseOk200);
        })
      );

      it('#edit edit goal', inject([HttpTestingController, HttpGoalService],
        (httpMock: HttpTestingController, service: HttpGoalService) => {

          // Arrange
          const id = '735f337f-755a-4db6-19ef-a092f0c82c8c';
          const name = 'test name';
          const done = true;

          // Act
          service.edit(id, name, done).subscribe(data => {

            // Assert
            expect(data).toEqual({});
          }, error => {

            // Assert
            expect(error.status).toEqual(404);
          });

          // Assert
          const request = httpMock.expectOne(`${service.baseUrl}/api/goal`);
          expect(request.request.method).toBe('PUT');
          request.flush({}, responseNotFound404);
        })
      );

    });

  }
}

