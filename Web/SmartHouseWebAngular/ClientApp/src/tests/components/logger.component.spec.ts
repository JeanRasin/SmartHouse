import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Logger, HttpLoggerService } from 'src/app/shared';
import { Observable, BehaviorSubject } from 'rxjs';
import { LoggerComponent } from 'src/app/logger/logger.component';
import { MatTableModule, MatPaginatorModule } from '@angular/material';

describe('Logger component', () => {
  const testData: Logger[] = [
    {
      logLevel: 5,
      eventId: {
        stateId: 4,
        name: 'oxkpcdlvch'
      },
      message: 'Metal Unbranded Plastic Hat calculating architect Buckinghamshire Cayman Islands Dollar payment revolutionary Massachusetts HTTP USB Music Money Market Account Kansas sky blue Metical XML Handmade Frozen Fish open-source Ergonomic',
      date: '1996-01-07T19:13:17.4527845',
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
      date: '1994-01-10T03:41:03.1171229',
      id: 'a649305f03a1407b2d1e7a0e619f7a71'
    }];

  const testDataInitial = Object.assign([], testData);

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        LoggerComponent
      ],
      imports: [
        MatTableModule,
        MatPaginatorModule
      ],
      schemas: [NO_ERRORS_SCHEMA],
    });
  });

  it('get logger data', () => {
    let serviceSpy = jasmine.createSpyObj('HttpLoggerService', ['get']);
    serviceSpy.get = (): Observable<Logger[]> => {
      const subject = new BehaviorSubject<Logger[]>(testData);
      return subject.asObservable();
    };

    TestBed.overrideComponent(LoggerComponent, {
      set: {
        providers: [
          { provide: HttpLoggerService, useValue: serviceSpy }
        ]
      }
    }).compileComponents();

    const fixture = TestBed.createComponent(LoggerComponent);
    const loggerComponent = fixture.debugElement.componentInstance;
    loggerComponent.ngOnInit();

    expect(loggerComponent.error).toBeNull();
    expect(loggerComponent.dataSource).not.toBeNull();
    expect(loggerComponent.dataSource.data).not.toEqual(testDataInitial);
  });

  it('get logger error', () => {
    const messageText = 'Error receiving data.';
    let serviceSpy = jasmine.createSpyObj('HttpLoggerService', ['get']);
    serviceSpy.get = (): Observable<Logger[]> => {
      const subject = new BehaviorSubject<Logger[]>(testData);
      subject.error({ message: messageText });
      return subject.asObservable();
    };

    TestBed.overrideComponent(LoggerComponent, {
      set: {
        providers: [
          { provide: HttpLoggerService, useValue: serviceSpy }
        ]
      }
    }).compileComponents();

    const fixture = TestBed.createComponent(LoggerComponent);
    const loggerComponent = fixture.debugElement.componentInstance;
    loggerComponent.ngOnInit();

    expect(loggerComponent.error).not.toBeNull();
    expect(loggerComponent.error).toEqual(messageText);
  });

});






