

using ClinicsAPP.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace ClinicsAPP.DTO
{
    public class DoctorRequestDTO

    {
        [Required(ErrorMessage = "Doctor Name can't be blank")]

        public string? FullName { get; set; }
        [Required(ErrorMessage = "Specialty can't be blank")]

        public string? Specalist { get; set; }
        [Required(ErrorMessage = "ClinicLocation can't be blank")]
  public string? Location { get; set; }
        public string? Description { set; get; }

        public decimal? Price { get; set; }
       // [RegularExpression(@"^\+9627[789]\d{7}$", ErrorMessage = "Invalid phone number")]
     //   public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public Guid UserId { get; set; }
       
       

    }

   /* public class UpdateDoctorDto
        {
            public string? FullName { get; set; }
            public string? Specialty { get; set; }
            public string? Location { get; set; }
        }
   */
      /// <summary>
    /// Converts the current object of PersonAddRequest into a new object of Person type
    /// </summary>
    /// <returns></returns>
    /*public Doctor ToDoctor()
        {
            return new Doctor() { PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), Address = Address, CountryID = CountryID, ReceiveNewsLetters = ReceiveNewsLetters };
        }*/
    }

