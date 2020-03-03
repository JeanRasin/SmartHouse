import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { WeatherComponent } from 'src/app/weather';
import { HttpWeatherService, Weather } from 'src/app/shared';
import { HttpClientTestingModule } from '@angular/common/http/testing';
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

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        WeatherComponent
      ],
      imports: [
        HttpClientTestingModule,
      ],
      schemas: [NO_ERRORS_SCHEMA],
    });
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
    }).compileComponents();

    const fixture = TestBed.createComponent(WeatherComponent);
    const weatherComponent = fixture.debugElement.componentInstance;
    weatherComponent.ngOnInit();

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










