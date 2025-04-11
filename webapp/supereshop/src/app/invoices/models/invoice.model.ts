export enum InvoiceStatus {
  Created = 0,
  Sent = 1,
  Unpaid = 2,
  PartiallyPaid = 3,
  Paid = 4,
  Overpaid = 5
}

export interface InvoiceItem {
  id: number;
  description: string | null;
  unitPrice: number;
  quantity: number;
  totalAmount: number;
}

export interface InvoicePayment {
  id: number;
  amount: number;
  paymentReference: string;
  paidAt: string;
}

export interface Invoice {
  id: number;
  number: string | null;
  customerName: string | null;
  totalAmount: number;
  paidAmount: number;
  remainingAmount: number;
  issueDate: string;
  dueDate: string;
  paymentReference: string | null;
  orderId: number | null;
  status: InvoiceStatus;
  createdAt: string;
  paidAt: string | null;
  items: InvoiceItem[] | null;
  payments: InvoicePayment[] | null;
}

export interface CreateInvoiceItemRequest {
  description: string;
  unitPrice: number;
  quantity: number;
}

export interface CreateInvoiceRequest {
  customerName: string;
  dueDate: string;
  items: CreateInvoiceItemRequest[];
  orderId?: number;
}

export interface PayInvoiceRequest {
  amount: number;
  paymentReference: string | null;
}
