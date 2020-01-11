import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WeatherComponent } from './weather.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

const goalRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'weather',
    component: WeatherComponent,
  }
]);

@NgModule({
  imports: [
    goalRouting,
    BrowserModule,
    HttpClientModule,
  ],
  exports: [WeatherComponent],
  declarations: [
    WeatherComponent
  ],
})
export class WeatherModule { }
