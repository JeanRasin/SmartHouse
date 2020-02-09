import { NgModule, ModuleWithProviders } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from '../home/home.component';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

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
    FormsModule
  ],
  exports: [
    FormsModule
  ],
  declarations: [
    HomeComponent
  ],
  //bootstrap: [HomeComponent],
  //entryComponents: [
  //  HomeComponent,
  //]
})
export class HomeModule { }
