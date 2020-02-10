import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RouterModule } from '@angular/router';
import { GoalDialogComponent } from './goal-helpers';
import { NavMenuComponent } from './nav-menu';

//import { ArticleListComponent, ArticleMetaComponent, ArticlePreviewComponent } from './article-helpers';
//import { FavoriteButtonComponent, FollowButtonComponent } from './buttons';
//import { ListErrorsComponent } from './list-errors.component';
//import { ShowAuthedDirective } from './show-authed.directive';

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
    GoalDialogComponent,
    NavMenuComponent
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
    NavMenuComponent,
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
