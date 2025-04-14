"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Invoice = void 0;
class Invoice {
    description = {
        displayName: 'Invoice',
        name: 'invoice',
        icon: 'file:invoice.svg',
        group: ['transform'],
        version: 1,
        subtitle: '={{$parameter["operation"] + ": " + $parameter["resource"]}}',
        description: 'Get invoice information',
        defaults: {
            name: 'Invoice',
        },
        inputs: ['main'],
        outputs: ['main'],
        credentials: [
            {
                name: 'invoiceApi',
                required: true,
            },
        ],
        properties: [
            {
                displayName: 'Operation',
                name: 'operation',
                type: 'options',
                noDataExpression: true,
                options: [
                    {
                        name: 'Get',
                        value: 'get',
                        description: 'Get an invoice',
                        action: 'Get an invoice',
                    },
                ],
                default: 'get',
            },
            {
                displayName: 'Invoice ID',
                name: 'invoiceId',
                type: 'string',
                default: '',
                required: true,
                displayOptions: {
                    show: {
                        operation: ['get'],
                    },
                },
                description: 'The ID of the invoice to get',
            },
        ],
    };
    async execute() {
        const items = this.getInputData();
        const returnData = [];
        const operation = this.getNodeParameter('operation', 0);
        const credentials = await this.getCredentials('invoiceApi');
        for (let i = 0; i < items.length; i++) {
            try {
                if (operation === 'get') {
                    const invoiceId = this.getNodeParameter('invoiceId', i);
                    const response = await globalThis.fetch(`${credentials.apiUrl}/api/invoices/${invoiceId}`, {
                        headers: {
                            'Authorization': `Bearer ${credentials.apiKey}`,
                            'Content-Type': 'application/json',
                        },
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const data = await response.json();
                    returnData.push({
                        json: data,
                        pairedItem: {
                            item: i,
                        },
                    });
                }
            }
            catch (error) {
                if (this.continueOnFail()) {
                    returnData.push({
                        json: {
                            error: error instanceof Error ? error.message : 'An unknown error occurred',
                        },
                        pairedItem: {
                            item: i,
                        },
                    });
                    continue;
                }
                throw error;
            }
        }
        return [returnData];
    }
}
exports.Invoice = Invoice;
//# sourceMappingURL=Invoice.node.js.map