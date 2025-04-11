import { InvoicesListComponent } from './invoices/invoices-list/invoices-list.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { Routes } from '@angular/router';
import { TasksListComponent } from './tasks/tasks-list/tasks-list.component';

export const routes: Routes = [
  {
    path: 'orders',
    component: OrdersListComponent
  },
  {
    path: 'tasks',
    component: TasksListComponent
  },
  {
    path: 'invoices',
    component: InvoicesListComponent
  },
  {
    path: '',
    redirectTo: 'orders',
    pathMatch: 'full'
  }
];
