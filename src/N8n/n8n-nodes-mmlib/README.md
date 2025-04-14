# n8n-nodes-mmlib

This package contains custom n8n nodes for MMLib.TestWorkflowEngine.

## Nodes

### Get Invoice
Gets invoice information from the MMLib.TestWorkflowEngine API.

### Order Created (Trigger)
Webhook trigger that handles order created events. Creates a webhook endpoint that can receive POST requests with order data.

## Build Instructions

1. Install dependencies:
```bash
npm install
```

2. Build the nodes:
```bash
npm run build
```

The compiled nodes will be in the `dist` folder.

## Development

### Prerequisites
- Node.js 18 or later
- npm

### Project Structure
```
n8n-nodes-mmlib/
├── nodes/
│   ├── Invoice/
│   │   ├── Invoice.node.ts
│   │   └── invoice.svg
│   └── OrderCreated/
│       ├── OrderCreated.node.ts
│       └── order.svg
├── package.json
└── tsconfig.json
```

### Local Testing with Aspire

The node is automatically mounted into n8n when running through Aspire. The node's `dist` folder is mounted to the n8n container at `/home/node/.n8n/custom/node_modules/n8n-nodes-mmlib`.

After making changes to the node:

1. Rebuild the node:
```bash
npm run build
```

2. Restart the Aspire application to load the changes:
```bash
dotnet run --project src/AppHost
```

## Notes

- The Get Invoice node ignores SSL certificate validation to work with self-signed certificates
- The Get Invoice API URL is hardcoded to `https://host.docker.internal:7257/invoices`
- The Order Created trigger creates a webhook endpoint that accepts POST requests 