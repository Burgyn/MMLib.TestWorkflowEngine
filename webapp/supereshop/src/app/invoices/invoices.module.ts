import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoicesRoutingModule } from './invoices-routing.module';
import { InvoicesListComponent } from './invoices-list/invoices-list.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    InvoicesRoutingModule,
    InvoicesListComponent
  ]
})
export class InvoicesModule { }
