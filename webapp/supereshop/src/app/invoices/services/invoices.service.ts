import { CreateInvoiceRequest, Invoice, PayInvoiceRequest } from '../models/invoice.model';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvoicesService {
  private apiUrl = 'http://localhost:5072';

  constructor(private http: HttpClient) { }

  getInvoices(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(`${this.apiUrl}/invoices`);
  }

  getInvoice(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiUrl}/invoices/${id}`);
  }

  createInvoice(request: CreateInvoiceRequest): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/invoices`, request);
  }

  payInvoice(id: number, request: PayInvoiceRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/invoices/${id}/pay`, request);
  }
}
