export enum OrderStatus {
  New = 0,
  InProgress = 1,
  OnHold = 2,
  Completed = 3,
  Cancelled = 4
}

export interface Order {
  id: number;
  customerName: string | null;
  description: string | null;
  unitPrice: number;
  quantity: number;
  totalAmount: number;
  status: OrderStatus;
  createdAt: string;
  lastModifiedAt: string | null;
}