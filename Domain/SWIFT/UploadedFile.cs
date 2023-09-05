namespace Domain.SWIFT;

public class UploadedFile
{
    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime DateUploaded { get; set; } = DateTime.Now;
}
