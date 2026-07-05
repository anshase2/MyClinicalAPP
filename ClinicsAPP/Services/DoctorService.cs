using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace ClinicsAPP.Services
{
    public class DoctorService 
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

  /*      private ResponseDoctorDTO MapToDTO(Doctor d)
        {
            return new ResponseDoctorDTO
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName,
                Specalist = d.Specalist,
                Description = d.Description,
                Rating = d.Rating,
                ReviewCount = d.ReviewCount,
                Location = d.Location,
                Phone = d.Phone,
                Price = d.Price,
                ImageUrl = d.ImageUrl,
                IsApproved = d.IsApproved,
                IsAvailable = d.IsAvailable
            };
        }*/

        public async Task<DoctorResponseDTO?> CreateDoctorAsync(DoctorRequestDTO dto)
        {
            if (dto.Specalist == null ||dto.FullName==null)
                return null;
            var doctor = new Doctor
            {
               
                UserId = dto.UserId,
                //doctor.user=getuserbyuserid(userid);
                FullName = dto.FullName,
                Specalist = dto.Specalist,
                Description = dto.Description,
                
                Location = dto.Location,
                Price = dto.Price,
               ImageUrl = dto.ImageUrl,
                Rating = 0,
                ReviewCount = 0,
                //IsApproved = false,
               // IsAvailable = true
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();

            return doctor.ToDoctorResponseDTO();
        }

        public async Task<List<DoctorResponseDTO?>> GetAllDoctorsAsync()
        {
            /*  var doctors = await _context.Doctors.Include(d => d.User).ToListAsync();
                  //Where(d => d.IsApproved).



              return doctors.Select(d =>d.ToDoctorResponseDTO()).ToList();*/
            var doctors = await _context.Doctors
         .Include(d => d.User)
         .ToListAsync();

            var result = new List<DoctorResponseDTO?>();

            foreach (var doctor in doctors)
            {
                var avgRating = await _context.Feedbacks
                    .Where(f => f.DoctorId == doctor.DoctorId)
                    .AverageAsync(f => (double?)f.Rating) ?? 0;

                result.Add(doctor.ToDoctorResponseDTO(Math.Round(avgRating, 1)));
            }

            return result;
        }

        public async Task<DoctorResponseDTO?> GetDoctorByIdAsync(int id)
        {

            var doctor = await _context.Doctors
                   .Include(d => d.User)
                   
                   .FirstOrDefaultAsync(d => d.DoctorId == id);
           
            return doctor == null ? null :doctor.ToDoctorResponseDTO();
        }

        public async Task<DoctorResponseDTO?> UpdateDoctorAsync(int Doctorid, DoctorRequestUpdated dto)
        {
           // var doctor = await _context.Doctors.FindAsync(id);
            var doctor = await _context.Doctors.FindAsync(Doctorid);

            if (doctor == null) return null;

        
            doctor.Specalist = dto.Specalist;
            doctor.Description = dto.Description;
            //doctor.rating=dto.rating
            //review
          
            doctor.Location = dto.Location;
            doctor.Price = dto.Price;
            doctor.ImageUrl = dto.ImageUrl;
            doctor.Rating=dto.Rating;
            doctor.ReviewCount = dto.ReviewCount;
           // doctor.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            return doctor.ToDoctorResponseDTO();
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<DoctorResponseDTO?>> GetDoctorsBySpecializationAsync(string spec)
        {
            var doctors = await _context.Doctors
                .Where(d => d.Specalist == spec)
                .ToListAsync();

            return doctors.Select(d => d.ToDoctorResponseDTO()).ToList();
        }

        public async Task<List<DoctorResponseDTO?>> GetAvailableDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Where(d => d.IsAvailable==true)
                .ToListAsync();

            return doctors.Select(d => d.ToDoctorResponseDTO()).ToList();
        }
        public async Task<List<DoctorResponseDTO?>> GetDoctorsByNameAsync(string name)
        {
            var doctors = await _context.Doctors
                .Where(d => d.FullName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return doctors.Select(d => d.ToDoctorResponseDTO()).ToList();
        }
        /* public async Task<bool> ApproveDoctorAsync(Guid id)
         {
             var doctor = await _context.Doctors.FindAsync(id);

             if (doctor == null) return false;

             doctor.IsApproved = true;
             await _context.SaveChangesAsync();

             return true;
         }*/
        //public async Task<bool> GetDoctorAppointmentAsync(Guid id)


        public async Task<DoctorResponseDTO?> GetDoctorByUserId(Guid userId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null) return null;

            var avgRating = await _context.Feedbacks
                .Where(f => f.DoctorId == doctor.DoctorId)
                .AverageAsync(f => (double?)f.Rating) ?? 0;

            return doctor.ToDoctorResponseDTO(Math.Round(avgRating, 1));
        }
        public async Task<int> GetDoctorAppointmentsCountAsync(int doctorId)
        {
            // Return the total number of appointments for the specified doctor.
            // Assumes an `Appointments` DbSet exists on the ApplicationDbContext
            // and that Appointment has a `DoctorId` property.
            return await _context.Appointments.CountAsync(a => a.DoctorId == doctorId);
        }
    }
}
