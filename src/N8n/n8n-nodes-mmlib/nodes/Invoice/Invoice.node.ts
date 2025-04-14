import {
	IDataObject,
	IExecuteFunctions,
	INodeExecutionData,
	INodeType,
	INodeTypeDescription,
} from 'n8n-workflow';

export class Invoice implements INodeType {
	description: INodeTypeDescription = {
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

	async execute(this: IExecuteFunctions): Promise<INodeExecutionData[][]> {
		const items = this.getInputData();
		const returnData: INodeExecutionData[] = [];
		const operation = this.getNodeParameter('operation', 0) as string;
		const credentials = await this.getCredentials('invoiceApi') as { apiUrl: string; apiKey: string };

		for (let i = 0; i < items.length; i++) {
			try {
				if (operation === 'get') {
					const invoiceId = this.getNodeParameter('invoiceId', i) as string;
					
					// Make API request to get invoice using global fetch
					const response = await globalThis.fetch(`${credentials.apiUrl}/api/invoices/${invoiceId}`, {
						headers: {
							'Authorization': `Bearer ${credentials.apiKey}`,
							'Content-Type': 'application/json',
						},
					});

					if (!response.ok) {
						throw new Error(`HTTP error! status: ${response.status}`);
					}

					const data = await response.json() as IDataObject;
					
					returnData.push({
						json: data,
						pairedItem: {
							item: i,
						},
					});
				}
			} catch (error) {
				if (this.continueOnFail()) {
					returnData.push({
						json: {
							error: error instanceof Error ? error.message : 'An unknown error occurred',
						} as IDataObject,
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