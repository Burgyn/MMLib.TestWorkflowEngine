import { Component, OnInit } from '@angular/core';
import { Order, OrderStatus } from '../models/order.model';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
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
    MatCardModule
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.css'
})
export class OrdersListComponent implements OnInit {
  orders: Order[] = [];
  displayedColumns: string[] = ['id', 'customerName', 'description', 'unitPrice', 'quantity', 'totalAmount', 'status', 'createdAt'];
  isLoading = true;
  OrderStatus = OrderStatus;

  constructor(private ordersService: OrdersService) {}

  ngOnInit(): void {
    this.loadOrders();
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
