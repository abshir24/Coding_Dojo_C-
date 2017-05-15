using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DojoConnections.Models
{

    public class User : Base{
        public int UserId{get; set; }
        
        [Required]
        [MinLength(2,ErrorMessage = "Name must be at least 2 characters long" )]
        public string Name { get; set; }
 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Password Confirmation must match!")]
        public string PasswordConfirmation { get; set; }

        public int NetworkId{get; set; }

        public string Description{get; set; }
    }
}