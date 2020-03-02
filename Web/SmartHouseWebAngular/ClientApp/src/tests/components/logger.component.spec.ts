import { ModuleWithProviders, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed, async } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { HttpWeatherService, HttpLoggerService } from 'src/app/shared';
import { RouterModule } from '@angular/router';
import { LoggerComponent } from 'src/app/logger/logger.component';
import { MatTableModule, MatPaginatorModule } from '@angular/material';

describe('Logger component !!!', () => {

  const logRouting: ModuleWithProviders = RouterModule.forChild([
    {
      path: 'log',
      component: LoggerComponent,
    }
  ]);

  const popupServiceStub = {
    open: () => {}
};

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        logRouting,
        BrowserModule,
        MatTableModule,
        MatPaginatorModule
      ],
      declarations: [
        LoggerComponent
      ],
      schemas: [NO_ERRORS_SCHEMA],
     //providers: [HttpLoggerService]
      providers: [{provide: HttpLoggerService, useValue: popupServiceStub } ]
      // providers: [{httpService: HttpLoggerService} ]
    }).compileComponents();
  }));

  // it('should create the logger', () => {
  //   const fixture = TestBed.createComponent(LoggerComponent);
  //   const app = fixture.debugElement.componentInstance;
  //   expect(app).toBeTruthy();
  // });

  // it('should called open', () => {
  //   const openSpy = jest.spyOn(popup, 'open');
  //   fixture.detectChanges();
  //   expect(openSpy).toHaveBeenCalled();
  // });

});






