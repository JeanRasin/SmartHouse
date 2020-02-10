import { ModuleWithProviders, NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { LoggerComponent } from './logger.component';

const goalRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'log',
    component: LoggerComponent,
  }
]);

@NgModule({
  imports: [
    goalRouting,
    BrowserModule,
    MatTableModule,
    MatPaginatorModule
  ],
  declarations: [
    LoggerComponent,
  ]
})
export class LoggerModule { }
