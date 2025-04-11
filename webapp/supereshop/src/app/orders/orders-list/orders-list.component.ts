import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Order, OrderStatus } from '../models/order.model';

import { CommonModule } from '@angular/common';
import { CreateOrderDialogComponent } from '../create-order-dialog/create-order-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { OrdersService } from '../services/orders.service';

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatProgressSpinnerModule,
    HttpClientModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.css'
})
export class OrdersListComponent implements OnInit {
  orders: Order[] = [];
  displayedColumns: string[] = ['id', 'customerName', 'description', 'unitPrice', 'quantity', 'totalAmount', 'status', 'createdAt'];
  isLoading = true;
  OrderStatus = OrderStatus;

  constructor(
    private ordersService: OrdersService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateOrderDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.ordersService.createOrder(result).subscribe({
          next: () => {
            this.loadOrders();
          },
          error: (error) => {
            console.error('Error creating order:', error);
          }
        });
      }
    });
  }

  private loadOrders(): void {
    this.ordersService.getOrders().subscribe({
      next: (data) => {
        this.orders = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading orders:', error);
        this.isLoading = false;
      }
    });
  }

  getStatusText(status: OrderStatus): string {
    return OrderStatus[status];
  }
}
