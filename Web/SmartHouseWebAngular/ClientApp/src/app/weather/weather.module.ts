import { ModuleWithProviders, NgModule } from '@angular/core';
//import { MatButtonModule } from '@angular/material/button';
//import { MatButtonToggleModule } from '@angular/material/button-toggle';
//import { MatPaginatorModule } from '@angular/material/paginator';
//import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
//import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
//import { SharedModule } from '../shared';
//import { GoalDialogComponent } from '../shared/goal-helpers';
import { WeatherComponent } from './weather.component';

const goalRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'Weather',
    component: WeatherComponent,
  }
]);

@NgModule({
  imports: [
    goalRouting,
    //FontAwesomeModule,
    //MatButtonToggleModule,
    //MatPaginatorModule,
    //MatTableModule,
    //MatButtonModule,
    //SharedModule
  ],
  declarations: [
    WeatherComponent
  ],
  //entryComponents: [
  //  GoalDialogComponent
  //]
})
export class WeatherModule { }
