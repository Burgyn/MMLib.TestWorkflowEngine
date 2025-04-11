namespace Tasks.Features.CreateTask;

public record CreateTaskRequest(
    string AssigneeEmail,
    string Description,
    DateTime StartDate,
    DateTime DueDate,
    int? OrderId); 