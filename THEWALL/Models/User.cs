using System.ComponentModel.DataAnnotations;
 
namespace THEWALL.Models
{
    public class User
    {
        [Required]
        [MinLength(2, ErrorMessage= "Your first name can't be less than 2 characters")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Your last name can't be less that 2 characters")]
        public string LastName { get; set; }
 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirmation {get; set;}
    }
}