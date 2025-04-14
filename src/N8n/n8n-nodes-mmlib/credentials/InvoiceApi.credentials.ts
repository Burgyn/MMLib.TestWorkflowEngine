import {
	ICredentialType,
	INodeProperties,
} from 'n8n-workflow';

export class InvoiceApi implements ICredentialType {
	name = 'invoiceApi';
	displayName = 'Invoice API';
	documentationUrl = '';
	properties: INodeProperties[] = [
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