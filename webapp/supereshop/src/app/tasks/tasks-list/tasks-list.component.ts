import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Task, TaskState } from '../models/task.model';

import { CommonModule } from '@angular/common';
import { CreateTaskDialogComponent } from '../create-task-dialog/create-task-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TasksService } from '../services/tasks.service';

@Component({
  selector: 'app-tasks-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatProgressSpinnerModule,
    HttpClientModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatTooltipModule
  ],
  templateUrl: './tasks-list.component.html',
  styleUrl: './tasks-list.component.css'
})
export class TasksListComponent implements OnInit {
  tasks: Task[] = [];
  displayedColumns: string[] = ['id', 'assigneeEmail', 'description', 'startDate', 'dueDate', 'state', 'orderId', 'createdAt', 'actions'];
  isLoading = true;
  TaskState = TaskState;

  constructor(
    private tasksService: TasksService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateTaskDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.tasksService.createTask(result).subscribe({
          next: () => {
            this.loadTasks();
          },
          error: (error) => {
            console.error('Error creating task:', error);
          }
        });
      }
    });
  }

  completeTask(taskId: number): void {
    this.tasksService.completeTask(taskId).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error completing task:', error);
      }
    });
  }

  getRowClass(state: TaskState): string {
    switch (state) {
      case TaskState.New:
        return 'state-new';
      case TaskState.InProgress:
        return 'state-in-progress';
      case TaskState.Completed:
        return 'state-completed';
      case TaskState.Cancelled:
        return 'state-cancelled';
      default:
        return '';
    }
  }

  private loadTasks(): void {
    this.tasksService.getTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.isLoading = false;
      }
    });
  }

  getStateText(state: TaskState): string {
    return TaskState[state];
  }

  canComplete(task: Task): boolean {
    return task.state === TaskState.New || task.state === TaskState.InProgress;
  }
}
