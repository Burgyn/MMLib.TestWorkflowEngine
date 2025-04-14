import {
	IDataObject,
	IExecuteFunctions,
	INodeExecutionData,
	INodeType,
	INodeTypeDescription,
} from 'n8n-workflow';

export class Invoice implements INodeType {
	description: INodeTypeDescription = {
		displayName: 'Get Invoice',
		name: 'invoice',
		icon: 'file:invoice.svg',
		group: ['transform'],
		version: 1,
		description: 'Get invoice information',
		defaults: {
			name: 'Get Invoice',
		},
		inputs: ['main'],
		outputs: ['main'],
		properties: [
			{
				displayName: 'Invoice ID',
				name: 'invoiceId',
				type: 'string',
				default: '',
				required: true,
				description: 'The ID of the invoice to get',
			},
		],
	};

	async execute(this: IExecuteFunctions): Promise<INodeExecutionData[][]> {
		const items = this.getInputData();
		const returnData: INodeExecutionData[] = [];
		const apiUrl = 'https://host.docker.internal:7257/invoices';

		// @ts-ignore
		process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

		for (let i = 0; i < items.length; i++) {
			try {
				const invoiceId = this.getNodeParameter('invoiceId', i) as string;
				
				// Make API request to get invoice using global fetch
				const response = await globalThis.fetch(`${apiUrl}/${invoiceId}`, {
					headers: {
						'Content-Type': 'application/json',
						'Accept': 'application/json',
					},
				});

				if (!response.ok) {
					throw new Error(`HTTP error! status: ${response.status}, message: ${await response.text()}`);
				}

				const data = await response.json() as IDataObject;
				
				returnData.push({
					json: data,
					pairedItem: {
						item: i,
					},
				});
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