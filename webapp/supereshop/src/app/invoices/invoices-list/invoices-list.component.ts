import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceStatus } from '../models/invoice.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { InvoiceItemsDialogComponent } from '../invoice-items-dialog/invoice-items-dialog.component';
import { InvoicesService } from '../services/invoices.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-invoices-list',
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
  templateUrl: './invoices-list.component.html',
  styleUrl: './invoices-list.component.css'
})
export class InvoicesListComponent implements OnInit {
  invoices: Invoice[] = [];
  displayedColumns: string[] = ['id', 'number', 'customerName', 'totalAmount', 'issueDate', 'dueDate', 'status', 'createdAt', 'actions'];
  isLoading = true;
  InvoiceStatus = InvoiceStatus;

  constructor(
    private invoicesService: InvoicesService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  private loadInvoices(): void {
    this.invoicesService.getInvoices().subscribe({
      next: (data) => {
        this.invoices = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading invoices:', error);
        this.isLoading = false;
      }
    });
  }

  getStatusText(status: InvoiceStatus): string {
    return InvoiceStatus[status];
  }

  viewItems(invoice: Invoice): void {
    if (invoice.items && invoice.items.length > 0) {
      this.dialog.open(InvoiceItemsDialogComponent, {
        data: { items: invoice.items },
        width: '600px'
      });
    }
  }

  getRowClass(status: InvoiceStatus): string {
    switch (status) {
      case InvoiceStatus.New:
        return 'state-new';
      case InvoiceStatus.Paid:
        return 'state-paid';
      case InvoiceStatus.Cancelled:
        return 'state-cancelled';
      default:
        return '';
    }
  }
}
