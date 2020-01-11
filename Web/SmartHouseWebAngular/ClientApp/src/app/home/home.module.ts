import { NgModule, ModuleWithProviders } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from '../home/home.component';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { WeatherModule } from '../weather/weather.module';

const goalRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  }
]);

@NgModule({
  imports: [
    goalRouting,
    BrowserModule,
    FormsModule,
    WeatherModule
  ],
  exports: [
    FormsModule
  ],
  declarations: [
    HomeComponent
  ],
})
export class HomeModule { }
