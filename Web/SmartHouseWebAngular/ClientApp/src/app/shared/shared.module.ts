import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RouterModule } from '@angular/router';
import { NavMenuComponent } from './nav-menu';
import { GoalDialogCreateComponent, GoalDialogEditComponent } from './goal-helpers';

@NgModule({
  imports: [
    MatFormFieldModule,
    MatDialogModule,
    MatInputModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
  ],
  declarations: [
    GoalDialogCreateComponent,
    GoalDialogEditComponent,
    NavMenuComponent
  ],
  exports: [
    CommonModule,
    NavMenuComponent,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
  ]
})
export class SharedModule { }
