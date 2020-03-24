import { ModuleWithProviders, NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SharedModule, GoalDialogCreateComponent, GoalDialogEditComponent } from '../shared';
import { GoalComponent } from './goal.component';
import { MatSortModule } from '@angular/material';

const goalRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'goal',
    component: GoalComponent,
  }
]);

@NgModule({
  imports: [
    goalRouting,
    FontAwesomeModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    MatTableModule,
    MatButtonModule,
    MatSortModule,
    SharedModule
  ],
  declarations: [
    GoalComponent
  ],
  entryComponents: [
    GoalDialogCreateComponent,
    GoalDialogEditComponent
  ]
})
export class GoalModule { }
