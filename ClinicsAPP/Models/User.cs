using System.ComponentModel.DataAnnotations;
namespace ClinicsAPP.Models
{
   
        public class User
        {
         public int UserId { get; set; }
        public string Email { get; set; }
       public string PasswordHash { get; set; } 
            public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? Gender { get; set; }
        public string Role { get; set; }


    }

       /* public class LoginViewModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }*/
    }



