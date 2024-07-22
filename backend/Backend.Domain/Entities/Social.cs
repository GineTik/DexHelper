using Backend.Domain.Entities.Base;

namespace Backend.Domain.Entities;

public class Social : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Url { get; set; } = default!;
}