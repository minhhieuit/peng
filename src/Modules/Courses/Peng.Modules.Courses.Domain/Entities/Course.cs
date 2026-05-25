using Peng.SharedKernel.Domain;

namespace Peng.Modules.Courses.Domain.Entities;

public class Course : Entity
{
    private readonly List<Enrollment> _enrollments = [];

    private Course() { }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? ThumbnailUrl { get; private set; }
    public decimal Price { get; private set; }
    public bool IsPublished { get; private set; }
    public Guid InstructorId { get; private set; }
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

    public static Course Create(string title, string description, decimal price, Guid instructorId)
        => new() { Title = title, Description = description, Price = price, InstructorId = instructorId };

    public void Update(string title, string description, decimal price, string? thumbnailUrl)
    {
        Title = title;
        Description = description;
        Price = price;
        ThumbnailUrl = thumbnailUrl;
        SetUpdatedAt();
    }

    public void Publish() => IsPublished = true;
    public void Unpublish() => IsPublished = false;
}
