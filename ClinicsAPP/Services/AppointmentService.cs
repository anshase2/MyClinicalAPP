using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading.Tasks;

namespace ClinicsAPP.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPatientService _patientService;



        public AppointmentService(ApplicationDbContext context, IPatientService patientService)
        {
            _context = context;
            _patientService = patientService;
        }

        public async Task<int> CleanupPastAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;
            var pastAppointments = await _context.Appointments
                .Where(a => a.AppointmentDate < now)
                .ToListAsync(cancellationToken);

            if (!pastAppointments.Any())
                return 0;

            _context.Appointments.RemoveRange(pastAppointments);
            var removed = await _context.SaveChangesAsync(cancellationToken);
            return removed;
        }

        public async Task<bool> RejectAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

            if (appointment == null)
                throw new InvalidOperationException("Appointment not found.");

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<AppointmentResponseDTO> CreateAppointmentAsync(CreateAppointmentRequestDTO request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var normalized = new DateTime(
 request.AppointmentDate.Year,
 request.AppointmentDate.Month,
 request.AppointmentDate.Day,
 request.AppointmentDate.Hour,
 0, 0);
            var exists = await _context.Appointments
     .AnyAsync(a =>
         a.AppointmentDate==normalized
         );

            if (exists)
            {
                throw new Exception("this date is already booked for another patient");
            }
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == request.PatientId, cancellationToken);
            if (!patientExists)
                throw new InvalidOperationException("Patient not found.");

            var doctorExists =  await _context.Doctors
    .FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId, cancellationToken);//anyasync
            if (doctorExists == null)
                throw new InvalidOperationException("Doctor not found.");

            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            //send notification to doctor and patient (optional)    

            return await MapToDto(appointment);
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

        public async Task<bool> HasAppointmentAsync(int patientId, int doctorId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.AnyAsync(
                a => a.PatientId == patientId && a.DoctorId == doctorId,
                cancellationToken);
        }

        private static async Task<AppointmentResponseDTO> MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            PatientId = a.PatientId,
            DoctorId = a.DoctorId,
           // PatientName = await _patientService.GetPatientNameAsync(a.PatientId) ?? "Unknown",
           // DoctorName = await _context.Doctors.Where(d => d.DoctorId == a.DoctorId).Select(d => d.FullName).FirstOrDefaultAsync(),
            AppointmentDate = a.AppointmentDate
           
        };
    }
}
