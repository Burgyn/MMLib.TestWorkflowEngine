namespace Tasks.Domain.Events;

public record TaskCompletedEvent(
    int Id,
    string AssigneeEmail,
    string Description,
    DateTime StartDate,
    DateTime DueDate,
    State State,
    int? OrderId,
    DateTime CreatedAt,
    DateTime CompletedAt); 