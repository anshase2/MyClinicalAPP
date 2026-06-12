using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicsAPP.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public PatientService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET ALL
        public async Task<List<PatientResponseDTO>> GetAllPatients()
        {
            return await _context.Patients
                .Select(p => new PatientResponseDTO
                {
                    PatientId = p.PatientId,
                    FullName = p.FullName!,
                    UserId = p.UserId
                })
                .ToListAsync();
        }
        // get patient by user id
        public async Task<PatientResponseDTO?> GetPatientByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new Exception("Invalid User Id");
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                throw new Exception("Patient not found");
            return new PatientResponseDTO
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName!,
                UserId = patient.UserId
            };
        }
        public async Task<PatientDetailsResponseDTO?> GetPatientDetailsByUserId(Guid userId)
        {


            var patient = await _context.Patients
     .Include(p => p.Appointments)
         .ThenInclude(a => a.Doctor)
             .ThenInclude(d => d.User)
     .Include(p => p.Feedbacks)
     .Include(p => p.User)
     .Include(p => p.Appointments)

     .FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
                throw new Exception("Patient not found");

            return new PatientDetailsResponseDTO
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName!,
                UserId = patient.UserId,
                Gender = patient.User.Gender,
              
                DateOfBirth = patient.User.DateOfBirth,
                Appointments = patient.Appointments.Select(a => new AppointmentResponseDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    DoctorName = a.Doctor.FullName
                }).ToList(),

                Feedbacks = patient.Feedbacks.Select(f => new FeedbackResponseDTO
                {
                    Id = f.Id,
                    Comment = f.Comment
                }).ToList()
            };
        }



        // GET BY ID
        public async Task<PatientResponseDTO?> GetPatientById(int id)
        {
           /* if (id <= 0)
                throw new Exception("Invalid Patient Id");*/

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                throw new Exception("Patient not found");

            return new PatientResponseDTO
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName!,
                UserId = patient.UserId
            };
        }



        // GET DETAILS (Appointments + Feedbacks)
        public async Task<PatientDetailsResponseDTO?> GetPatientDetails(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Patient Id");

            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Feedbacks)
                .Include(p => p.User) 
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                throw new Exception("Patient not found");

            return new PatientDetailsResponseDTO
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName!,
                UserId = patient.UserId,
                Gender=patient.User.Gender,
                DateOfBirth=patient.User.DateOfBirth,
                Appointments = patient.Appointments.Select(a => new AppointmentResponseDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    DoctorName = a.Doctor.FullName


                }).ToList(),

                Feedbacks = patient.Feedbacks.Select(f => new FeedbackResponseDTO
                {
                    Id = f.Id,
                    Comment = f.Comment
                }).ToList()
            };
        }

        // CREATE
        public async Task<int> CreatePatient(PatientRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("FullName is required");

            // ✔ check user using Identity
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user == null)
                throw new Exception("User not found");

            // ✔ prevent duplicate patient
            var alreadyExists = await _context.Patients
                .AnyAsync(p => p.UserId == dto.UserId);

            if (alreadyExists)
                throw new Exception("Patient already exists for this user");

            var patient = new Patient
            {
                FullName = dto.FullName,
                UserId = dto.UserId
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient.PatientId;
        }
        // DELETE
        public async Task<bool> DeletePatient(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid Patient Id");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                throw new Exception("Patient not found");

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}