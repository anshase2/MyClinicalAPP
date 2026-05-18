using ClinicsAPP.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ClinicsAPP.DTO
{
    

    
        public class RegisterDTO
    {
        [Required(ErrorMessage = "First name can't be blank")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name can't be blank")]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
            [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
            public string Email { get; set; }


            [Required(ErrorMessage = "Phone can't be blank")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only")]
            [DataType(DataType.PhoneNumber)]
            public string Phone { get; set; }


            [Required(ErrorMessage = "Password can't be blank")]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Required(ErrorMessage = "Confirm Password can't be blank")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Password and confirmation do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        public UserTypeOptions Role { get; set; }
    }
    }

