# Create topics
$topics = @(
    "order-created",
    "order-status-changed",
    "order-completed",
    "invoice-created",
    "invoice-paid",
    "task-created",
    "task-completed"
)

foreach ($topic in $topics) {
    Write-Host "Creating topic: $topic"
    az servicebus topic create --name $topic --namespace-name $env:ServiceBusNamespace --resource-group $env:ResourceGroup
    
    # Create default subscription for each topic
    $subscriptionName = "$topic-sub"
    Write-Host "Creating subscription: $subscriptionName for topic: $topic"
    az servicebus topic subscription create --name $subscriptionName --topic-name $topic --namespace-name $env:ServiceBusNamespace --resource-group $env:ResourceGroup
} 