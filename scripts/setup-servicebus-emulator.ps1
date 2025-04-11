$topics = @(
    "order-created",
    "order-status-changed",
    "order-completed",
    "invoice-created",
    "invoice-paid",
    "task-created",
    "task-completed"
)

$baseUrl = "http://localhost:9090" # Default Service Bus Emulator management endpoint

foreach ($topic in $topics) {
    Write-Host "Creating topic: $topic"
    
    # Create topic
    $topicUrl = "$baseUrl/topics/$topic"
    try {
        Invoke-RestMethod -Uri $topicUrl -Method Put
        Write-Host "Created topic: $topic"
        
        # Create default subscription
        $subscriptionName = "$topic-sub"
        $subscriptionUrl = "$topicUrl/subscriptions/$subscriptionName"
        Invoke-RestMethod -Uri $subscriptionUrl -Method Put
        Write-Host "Created subscription: $subscriptionName for topic: $topic"
    }
    catch {
        Write-Warning "Failed to create topic/subscription: $topic. Error: $_"
    }
} 