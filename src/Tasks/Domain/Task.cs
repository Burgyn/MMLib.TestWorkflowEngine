namespace Tasks.Domain;

public class Task
{
    public int Id { get; set; }

    public required string AssigneeEmail { get; set; }

    public required string Description { get; set; }

    public required DateTime StartDate { get; set; }

    public required DateTime DueDate { get; set; }

    public State State { get; set; }

    public int? OrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastModifiedAt { get; set; }
} 