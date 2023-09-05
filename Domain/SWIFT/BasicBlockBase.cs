namespace Domain.SWIFT;

/// <summary>
/// Basic SWIFT message block properties
/// </summary>
public class BasicBlockBase
{
    public int Id { get; set; }

    public int FileId { get; set; }

    public string Value { get; set; }
}
