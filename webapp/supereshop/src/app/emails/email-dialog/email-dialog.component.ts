import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-email-dialog',
  templateUrl: './email-dialog.component.html',
  styleUrls: ['./email-dialog.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class EmailDialogComponent {
  emailForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<EmailDialogComponent>,
    private fb: FormBuilder
  ) {
    this.emailForm = this.fb.group({
      message: ['', [Validators.required, Validators.minLength(1)]]
    });
  }

  onSubmit(): void {
    if (this.emailForm.valid) {
      this.dialogRef.close(this.emailForm.value.message);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
