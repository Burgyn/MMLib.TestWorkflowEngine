import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { InvoicePayment } from '../models/invoice.model';

@Component({
  selector: 'app-invoice-payments-dialog',
  templateUrl: './invoice-payments-dialog.component.html',
  styleUrls: ['./invoice-payments-dialog.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatTableModule
  ]
})
export class InvoicePaymentsDialogComponent {
  displayedColumns: string[] = ['amount', 'paymentReference', 'paidAt'];
  payments: InvoicePayment[];

  constructor(
    public dialogRef: MatDialogRef<InvoicePaymentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { payments: InvoicePayment[] }
  ) {
    this.payments = data.payments;
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
