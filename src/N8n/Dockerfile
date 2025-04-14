FROM n8nio/n8n:latest

USER root

# Install global dependencies
RUN npm install -g typescript

# Create custom nodes directory with correct permissions
RUN mkdir -p /home/node/.n8n/custom && \
    chown -R node:node /home/node/.n8n && \
    chmod 750 /home/node/.n8n

# First, copy the entire node project and build it
COPY n8n-nodes-mmlib /tmp/n8n-nodes-mmlib
WORKDIR /tmp/n8n-nodes-mmlib

# Set permissions for build directory
RUN chown -R node:node /tmp/n8n-nodes-mmlib

# Build as node user
USER node
RUN npm install && npm run build

# Copy only the necessary files from dist to custom directory
USER root
RUN mkdir -p /home/node/.n8n/custom/n8n-nodes-mmlib && \
    cp -r /tmp/n8n-nodes-mmlib/dist/nodes /home/node/.n8n/custom/n8n-nodes-mmlib/ && \
    cp -r /tmp/n8n-nodes-mmlib/dist/credentials /home/node/.n8n/custom/n8n-nodes-mmlib/ && \
    cp /tmp/n8n-nodes-mmlib/package.json /home/node/.n8n/custom/n8n-nodes-mmlib/ && \
    chown -R node:node /home/node/.n8n/custom/n8n-nodes-mmlib

# Clean up
RUN rm -rf /tmp/n8n-nodes-mmlib

# Switch to node user for npm operations
USER node
WORKDIR /home/node/.n8n/custom/n8n-nodes-mmlib

# Install production dependencies
RUN npm install --production

# Create n8n config with proper permissions
RUN mkdir -p /home/node/.n8n && \
    echo '{"nodes": {"include": ["n8n-nodes-base", "n8n-nodes-mmlib"]}}' > /home/node/.n8n/.custom-nodes.json && \
    chmod 600 /home/node/.n8n/config || true && \
    chmod 600 /home/node/.n8n/.custom-nodes.json

# Return to default workdir
WORKDIR /data

# Set environment variables for custom nodes
ENV N8N_CUSTOM_EXTENSIONS="/home/node/.n8n/custom"
ENV N8N_NODES_INCLUDE="n8n-nodes-base,n8n-nodes-mmlib"
ENV N8N_NODES_EXCLUDE_FROM_INCLUDE=""
ENV N8N_LOG_LEVEL=debug
ENV NODE_PATH=/home/node/.n8n/custom/n8n-nodes-mmlib/node_modules
ENV N8N_ENFORCE_SETTINGS_FILE_PERMISSIONS=true 