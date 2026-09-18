using System.ComponentModel.DataAnnotations;

namespace CheckPoint2App.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailId { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ContactNo { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    public string? Province { get; set; }

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;
}