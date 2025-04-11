import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { InvoiceItem } from '../models/invoice.model';

@Component({
  selector: 'app-invoice-items-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatTableModule
  ],
  templateUrl: './invoice-items-dialog.component.html',
  styleUrl: './invoice-items-dialog.component.css'
})
export class InvoiceItemsDialogComponent {
  displayedColumns: string[] = ['description', 'unitPrice', 'quantity', 'totalAmount'];

  constructor(
    public dialogRef: MatDialogRef<InvoiceItemsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { items: InvoiceItem[] }
  ) {}

  onClose(): void {
    this.dialogRef.close();
  }
}
