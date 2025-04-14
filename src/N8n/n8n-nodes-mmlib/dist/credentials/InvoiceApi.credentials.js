"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.InvoiceApi = void 0;
class InvoiceApi {
    name = 'invoiceApi';
    displayName = 'Invoice API';
    documentationUrl = '';
    properties = [
        {
            displayName: 'API URL',
            name: 'apiUrl',
            type: 'string',
            default: '',
            required: true,
        },
        {
            displayName: 'API Key',
            name: 'apiKey',
            type: 'string',
            typeOptions: {
                password: true,
            },
            default: '',
            required: true,
        },
    ];
}
exports.InvoiceApi = InvoiceApi;
//# sourceMappingURL=InvoiceApi.credentials.js.map