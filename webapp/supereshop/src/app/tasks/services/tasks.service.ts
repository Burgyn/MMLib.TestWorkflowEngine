import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';

export interface CreateTaskRequest {
  assigneeEmail: string | null;
  description: string | null;
  startDate: string;
  dueDate: string;
  orderId: number | null;
}

export interface CompleteTaskRequest {
  completedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private apiUrl = 'http://localhost:5124';

  constructor(private http: HttpClient) { }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.apiUrl}/tasks`);
  }

  createTask(request: CreateTaskRequest): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/tasks`, request);
  }

  completeTask(id: number): Observable<void> {
    const request: CompleteTaskRequest = {
      completedAt: new Date().toISOString()
    };
    return this.http.post<void>(`${this.apiUrl}/tasks/${id}/complete`, request);
  }
}
