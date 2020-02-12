import { ModuleWithProviders, NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { LoggerComponent } from './logger.component';

const logRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'log',
    component: LoggerComponent,
  }
]);

@NgModule({
  imports: [
    logRouting,
    BrowserModule,
    MatTableModule,
    MatPaginatorModule
  ],
  declarations: [
    LoggerComponent,
  ]
})
export class LoggerModule { }
