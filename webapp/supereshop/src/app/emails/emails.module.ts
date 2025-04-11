import { RouterModule, Routes } from '@angular/router';

import { EmailsComponent } from './emails.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: EmailsComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class EmailsModule { }
