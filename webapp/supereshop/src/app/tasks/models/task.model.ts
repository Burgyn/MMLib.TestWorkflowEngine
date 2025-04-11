export enum TaskState {
  New = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3
}

export interface Task {
  id: number;
  assigneeEmail: string | null;
  description: string | null;
  startDate: string;
  dueDate: string;
  state: TaskState;
  orderId: number | null;
  createdAt: string;
  lastModifiedAt: string | null;
}
