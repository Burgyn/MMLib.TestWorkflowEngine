import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../models/order.model';

export interface CreateOrderRequest {
  customerName: string | null;
  description: string | null;
  unitPrice: number;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl = 'http://localhost:5150';

  constructor(private http: HttpClient) { }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/orders`);
  }

  createOrder(request: CreateOrderRequest): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/orders`, request);
  }
}
