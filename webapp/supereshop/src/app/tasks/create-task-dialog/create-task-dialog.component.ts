import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CreateTaskRequest } from '../services/tasks.service';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-create-task-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './create-task-dialog.component.html',
  styleUrl: './create-task-dialog.component.css'
})
export class CreateTaskDialogComponent {
  taskForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<CreateTaskDialogComponent>,
    private fb: FormBuilder
  ) {
    this.taskForm = this.fb.group({
      assigneeEmail: [''],
      description: [''],
      startDate: ['', Validators.required],
      dueDate: ['', Validators.required],
      orderId: [null]
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const request: CreateTaskRequest = {
        ...formValue,
        startDate: formValue.startDate.toISOString(),
        dueDate: formValue.dueDate.toISOString()
      };
      this.dialogRef.close(request);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
