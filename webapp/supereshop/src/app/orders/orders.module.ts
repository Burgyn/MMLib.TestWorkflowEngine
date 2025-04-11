import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersListComponent } from './orders-list/orders-list.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    OrdersListComponent
  ]
})
export class OrdersModule { }
