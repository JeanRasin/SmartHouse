import { ModuleWithProviders, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed, async } from '@angular/core/testing';
import { WeatherComponent } from 'src/app/weather';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { HttpWeatherService, Weather } from 'src/app/shared';
import { RouterModule } from '@angular/router';
import { LoggerComponent } from 'src/app/logger/logger.component';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { Observable, BehaviorSubject } from 'rxjs';

describe('Weather component', () => {
  const testData: Weather = {
    windSpeed: 300,
    windDeg: 290,
    temp: -1,
    city: 'Perm',
    pressure: 977,
    humidity: 100,
    description: 'light snow; mist',
    feelsLike: -5.23
  };

  let serviceSpy: HttpWeatherService;

  // const dataStub = (): Observable<Weather> => {
  //   const subject = new BehaviorSubject<Weather>(testData);
  //   const result = subject.asObservable();
  //   return result;
  //   //  return Observable.of(testData);
  // };

  beforeEach(() => {


    TestBed.configureTestingModule({
      declarations: [
        WeatherComponent
      ],
      imports: [
        HttpClientTestingModule,
      ],
      // providers: [
      //   //  HttpWeatherService
      //   { provide: HttpWeatherService, useValue: serviceSpy },
      // ],
      schemas: [NO_ERRORS_SCHEMA],
    });
    // .overrideComponent(WeatherComponent, {
    //   set: {
    //     providers: [
    //       { provide: HttpWeatherService, useValue: serviceSpy }
    //     ]
    //   }
    // })
    // .compileComponents();
  });

  it('get weather data', () => {
    let serviceSpy = jasmine.createSpyObj('HttpWeatherService', ['get']);
    serviceSpy.get = (): Observable<Weather> => {
      const subject = new BehaviorSubject<Weather>(testData);
      return subject.asObservable();
    };

    TestBed.overrideComponent(WeatherComponent, {
      set: {
        providers: [
          { provide: HttpWeatherService, useValue: serviceSpy }
        ]
      }
    })
      .compileComponents();

    const fixture = TestBed.createComponent(WeatherComponent);
    const weatherComponent = fixture.debugElement.componentInstance;
    weatherComponent.ngOnInit();
    // fixture.whenStable();

    debugger;
    expect(weatherComponent.error).toBeNull();
    expect(weatherComponent.weatherData).toEqual(testData);

  });

  it('get weather error', () => {
    const messageText = 'Error receiving data.';
    let serviceSpy = jasmine.createSpyObj('HttpWeatherService', ['get']);
    serviceSpy.get = (): Observable<Weather> => {
      const subject = new BehaviorSubject<Weather>(null);
      subject.error({ message: messageText });
      return subject.asObservable();
    };

    TestBed.overrideComponent(WeatherComponent, {
      set: {
        providers: [
          { provide: HttpWeatherService, useValue: serviceSpy }
        ]
      }
    }).compileComponents();

    const fixture = TestBed.createComponent(WeatherComponent);
    const weatherComponent = fixture.debugElement.componentInstance;
    weatherComponent.ngOnInit();

    expect(weatherComponent.error).not.toBeNull();
    expect(weatherComponent.error).toEqual(messageText);
  });


});










