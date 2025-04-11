export enum InvoiceStatus {
  New = 0,
  Paid = 1,
  Cancelled = 2
}

export interface InvoiceItem {
  id: number;
  description: string | null;
  unitPrice: number;
  quantity: number;
  totalAmount: number;
}

export interface Invoice {
  id: number;
  number: string | null;
  customerName: string | null;
  totalAmount: number;
  issueDate: string;
  dueDate: string;
  paymentReference: string | null;
  orderId: number | null;
  status: InvoiceStatus;
  createdAt: string;
  paidAt: string | null;
  items: InvoiceItem[] | null;
}

export interface CreateInvoiceItemRequest {
  description: string | null;
  unitPrice: number;
  quantity: number;
}

export interface CreateInvoiceRequest {
  customerName: string | null;
  dueDate: string;
  items: CreateInvoiceItemRequest[] | null;
  orderId: number | null;
}

export interface PayInvoiceRequest {
  paymentReference: string | null;
}
