import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CreateOrderRequest } from '../services/orders.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-create-order-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './create-order-dialog.component.html',
  styleUrl: './create-order-dialog.component.css'
})
export class CreateOrderDialogComponent {
  orderForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<CreateOrderDialogComponent>,
    private fb: FormBuilder
  ) {
    this.orderForm = this.fb.group({
      customerName: [''],
      description: [''],
      unitPrice: ['', [Validators.required, Validators.min(0)]],
      quantity: ['', [Validators.required, Validators.min(1)]]
    });
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      const request: CreateOrderRequest = this.orderForm.value;
      this.dialogRef.close(request);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
