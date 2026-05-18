using System;
using ClinicsAPP.Models;

namespace ClinicsAPP.DTO
{
    public class DoctorResponseDTO
    {

        public Guid DoctorId { get; set; }
        public string? FullName { set; get; }
        public string? Specalist { set; get; }
        public string? Description { set; get; }



        public double? Rating { get; set; }
        public int ReviewCount { get; set; }
        public string? Location { get; set; }
     
        public Guid UserId { get; set; }

        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        // public bool IsApproved { get; set; }
        public bool? IsAvailable { get; set; }


    }
    public static class DoctorExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Doctor class into DoctorResponse class
        /// </summary>
        /// <param name="Doctor">The Doctor object to convert</param>
        /// /// <returns>Returns the converted DoctorResponse object</returns>
         public static DoctorResponseDTO? ToDoctorResponseDTO(this Doctor Doctor)
         {
             //person => convert => PersonResponse
             if(Doctor.Specalist==null||Doctor.DoctorId==Guid.Empty||Doctor.Location==null)
                return null;
             return new DoctorResponseDTO()
             {

                 DoctorId = Doctor.DoctorId,
                 FullName = Doctor.FullName,
                 
                 Location=Doctor.Location, Specalist = Doctor.Specalist,   
                 Description = Doctor.Description,  
                 Rating = Doctor.Rating,
                 ReviewCount = Doctor.ReviewCount,
                 Price = Doctor.Price,
                 ImageUrl = Doctor.ImageUrl,
                 UserId = Doctor.UserId,
                 
                
                 

                 IsAvailable=Doctor.IsAvailable,

             };
         }
   

    }
}
