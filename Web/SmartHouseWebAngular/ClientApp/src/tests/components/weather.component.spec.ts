import { ModuleWithProviders, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed, async } from "@angular/core/testing";
import { WeatherComponent } from "src/app/weather";
import { BrowserModule } from "@angular/platform-browser";
import { HttpClientModule } from "@angular/common/http";
import { HttpWeatherService } from "src/app/shared";
import { RouterModule } from '@angular/router';


describe('Weather component', () => {

  const goalRouting: ModuleWithProviders = RouterModule.forChild([
    {
      path: 'weather',
      component: WeatherComponent,
    }
  ]);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
    //   imports: [WeatherComponent
    //  ],
    //   declarations: [
    //     WeatherComponent
    //   ],

    imports: [
      goalRouting,
      BrowserModule,
      HttpClientModule,
    ],
    
    //exports: [WeatherComponent],
    declarations: [
      WeatherComponent
    ],
   // schemas:      [ NO_ERRORS_SCHEMA ]
   // providers : [HttpWeatherService]
    }).compileComponents();
  }));
  
});

it('should create the wheare component', () => {
 // debugger;
  const fixture = TestBed.createComponent(WeatherComponent);
  const app = fixture.debugElement.componentInstance;
  expect(app).toBeTruthy();
});


/*
describe('rrr', () => {
  var loggerComponent;
  let comp;

  beforeEach(async(() => {
    loggerComponent = TestBed.createComponent(LoggerComponent);
    comp = loggerComponent.componentInstance;
  }));

  it('333', () => {
    expect(comp.dataSource).toBeTruthy();
  });

  it('444', () => {
    expect(comp.dataSource).toEqual(loggerComponent);
  });
  console.log('777');
});

*/








