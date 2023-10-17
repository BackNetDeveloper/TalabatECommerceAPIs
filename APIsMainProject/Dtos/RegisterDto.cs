using System.ComponentModel.DataAnnotations;

namespace APIsMainProject.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplyName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&])([a-zA-Z0-9@$!%*?&]{5,})$",
            ErrorMessage = "At least 5 characters long,One lowercase,one uppercase,one number,one special character and No whitespaces")]
        public string Password { get; set; }
    }
}
