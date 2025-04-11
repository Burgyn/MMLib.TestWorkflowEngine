using Tasks.Domain;

namespace Tasks.Features.GetTask;

public record GetTaskResponse(
    int Id,
    string AssigneeEmail,
    string Description,
    DateTime StartDate,
    DateTime DueDate,
    State State,
    int? OrderId,
    DateTime CreatedAt,
    DateTime? LastModifiedAt); 