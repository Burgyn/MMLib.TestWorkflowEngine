import { Component, OnInit } from '@angular/core';
import { Invoice, InvoiceStatus } from '../models/invoice.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { InvoiceItemsDialogComponent } from '../invoice-items-dialog/invoice-items-dialog.component';
import { InvoicePaymentsDialogComponent } from '../invoice-payments-dialog/invoice-payments-dialog.component';
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
  displayedColumns: string[] = ['id', 'number', 'customerName', 'totalAmount', 'paidAmount', 'remainingAmount', 'issueDate', 'dueDate', 'status', 'createdAt', 'actions'];
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
    switch (status) {
      case InvoiceStatus.Created:
        return 'Vytvorená';
      case InvoiceStatus.Sent:
        return 'Odoslaná';
      case InvoiceStatus.Unpaid:
        return 'Neuhradená';
      case InvoiceStatus.PartiallyPaid:
        return 'Čiastočne uhradená';
      case InvoiceStatus.Paid:
        return 'Uhradená';
      case InvoiceStatus.Overpaid:
        return 'Preplatená';
      default:
        return 'Neznámy stav';
    }
  }

  viewItems(invoice: Invoice): void {
    if (invoice.items && invoice.items.length > 0) {
      this.dialog.open(InvoiceItemsDialogComponent, {
        data: { items: invoice.items },
        width: '600px'
      });
    }
  }

  viewPayments(invoice: Invoice): void {
    if (invoice.payments && invoice.payments.length > 0) {
      this.dialog.open(InvoicePaymentsDialogComponent, {
        data: { payments: invoice.payments },
        width: '600px'
      });
    }
  }

  getRowClass(status: InvoiceStatus): string {
    switch (status) {
      case InvoiceStatus.Created:
        return 'state-created';
      case InvoiceStatus.Sent:
        return 'state-sent';
      case InvoiceStatus.Unpaid:
        return 'state-unpaid';
      case InvoiceStatus.PartiallyPaid:
        return 'state-partially-paid';
      case InvoiceStatus.Paid:
        return 'state-paid';
      case InvoiceStatus.Overpaid:
        return 'state-overpaid';
      default:
        return '';
    }
  }
}
