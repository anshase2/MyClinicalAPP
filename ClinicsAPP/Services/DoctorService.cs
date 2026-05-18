using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Data;
using Microsoft.EntityFrameworkCore;
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
            if (dto.Specalist == null || dto.Location == null||dto.FullName==null)
                return null;
            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                UserId = dto.userid,
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
            var doctors = await _context.Doctors.ToListAsync();
                //Where(d => d.IsApproved).
            


            return doctors.Select(d =>d.ToDoctorResponseDTO()).ToList();
        }

        public async Task<DoctorResponseDTO?> GetDoctorByIdAsync(Guid id)
        {

            var doctor = await _context.Doctors.FindAsync(id);

            return doctor == null ? null :doctor.ToDoctorResponseDTO();
        }

        public async Task<DoctorResponseDTO?> UpdateDoctorAsync(Guid Doctorid, DoctorRequestUpdated dto)
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
            doctor.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            return doctor.ToDoctorResponseDTO();
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
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


           }
}
