import {
	INodeType,
	INodeTypeDescription,
	IWebhookFunctions,
	IWebhookResponseData,
} from 'n8n-workflow';

export class OrderCreated implements INodeType {
	description: INodeTypeDescription = {
		displayName: 'Order Created',
		name: 'orderCreated',
		icon: 'file:order.svg',
		group: ['trigger'],
		version: 1,
		description: 'Handle order created webhook events',
		defaults: {
			name: 'Order Created',
		},
		inputs: [],
		outputs: ['main'],
		webhooks: [
			{
				name: 'default',
				httpMethod: 'POST',
				responseMode: 'onReceived',
				path: '={{$parameter["path"]}}',
				isFullPath: false,
				webhookDescription: 'Receive order created events',
			},
		],
		properties: [
			{
				displayName: 'Path',
				name: 'path',
				type: 'string',
				default: 'order-created',
				description: 'The path to use for the webhook URL (e.g. order-created)',
				required: true,
			},
			{
				displayName: 'Authentication',
				name: 'authentication',
				type: 'boolean',
				default: true,
				description: 'If authentication should be enabled for the webhook',
			},
		],
	};

	async webhook(this: IWebhookFunctions): Promise<IWebhookResponseData> {
		const req = this.getRequestObject();
		const body = req.body;

		if (!body) {
			throw new Error('No body received in webhook request');
		}

		return {
			workflowData: [
				[
					{
						json: body,
					},
				],
			],
		};
	}
} 