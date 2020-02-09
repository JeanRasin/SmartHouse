import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RouterModule } from '@angular/router';
import { GoalDialogComponent } from './goal-helpers';
import { MatInputModule } from '@angular/material';

//import { ArticleListComponent, ArticleMetaComponent, ArticlePreviewComponent } from './article-helpers';
//import { FavoriteButtonComponent, FollowButtonComponent } from './buttons';
//import { ListErrorsComponent } from './list-errors.component';
//import { ShowAuthedDirective } from './show-authed.directive';

@NgModule({
  imports: [
    MatFormFieldModule,
    MatDialogModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MatInputModule
  ],
  declarations: [
    GoalDialogComponent,
    //ArticleMetaComponent,
    //ArticlePreviewComponent,
    //FavoriteButtonComponent,
    //FollowButtonComponent,
    //ListErrorsComponent,
    //ShowAuthedDirective
  ],
  exports: [
    //ArticleListComponent,
    //ArticleMetaComponent,
    //ArticlePreviewComponent,
    CommonModule,
    //FavoriteButtonComponent,
    //FollowButtonComponent,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    //ListErrorsComponent,
    RouterModule,
    //ShowAuthedDirective
  ]
})
export class SharedModule { }
