using System.ComponentModel.DataAnnotations;

public class LoginDTO
{
    [Required]
    public string email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string password { get; set; }
}