using System.ComponentModel.DataAnnotations;

public class Contact
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Topic { get; set; }

    public string Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
