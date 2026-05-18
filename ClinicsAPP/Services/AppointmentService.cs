using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicsAPP.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentResponseDTO> CreateAppointmentAsync(int patientId, Guid doctorId, DateTime appointmentDate, CancellationToken cancellationToken = default)
        {
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == patientId, cancellationToken);
            if (!patientExists)
                throw new InvalidOperationException("Patient not found.");

            var doctorExists = await _context.Doctors.AnyAsync(d => d.DoctorId == doctorId, cancellationToken);
            if (!doctorExists)
                throw new InvalidOperationException("Doctor not found.");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(appointment);
        }

        public async Task<IReadOnlyList<AppointmentResponseDTO>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var list = await _context.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new AppointmentResponseDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate
                })
                .ToListAsync();

            return list;
        }

        public async Task<bool> HasAppointmentAsync(int patientId, Guid doctorId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.AnyAsync(
                a => a.PatientId == patientId && a.DoctorId == doctorId,
                cancellationToken);
        }

        private static AppointmentResponseDTO MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            PatientId = a.PatientId,
            DoctorId = a.DoctorId,
            AppointmentDate = a.AppointmentDate
        };
    }
}
