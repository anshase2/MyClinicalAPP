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
        private readonly INotificationService _notificationService;



        public AppointmentService(ApplicationDbContext context, IPatientService patientService, INotificationService notificationService)
        {
            _notificationService = notificationService;
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
            var appointment = await _context.Appointments.Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

            if (appointment == null)
                return false;
            appointment.Status = "Rejected";
           
            await _context.SaveChangesAsync(cancellationToken);
            await _notificationService.SendAsync(appointment.Patient.UserId, "Appointment Rejected", $"Your appointment on {appointment.AppointmentDate} has been rejected.", "dsad");
            return true;
        }

        public async Task<bool> AcceptAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments.Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

            if (appointment == null)
               return false;
            appointment.Status = "Accepted";

            await _context.SaveChangesAsync(cancellationToken);
            await _notificationService.SendAsync(appointment.Patient.UserId, "Appointment Accepted", $"Your appointment on {appointment.AppointmentDate} has been accepted.", "dsad");
            return true;
        }
        public async Task<bool> CancelAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments.Include(a=>a.Patient).Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

            if (appointment == null)
                return false;
            appointment.Status = "Cancelled";

            await _context.SaveChangesAsync(cancellationToken);
            await _notificationService.SendAsync(appointment.Patient.UserId, "Appointment Cancelled", $"Your appointment on {appointment.AppointmentDate} has been cancelled.", "dsad");
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
            /*var exists = await _context.Appointments
     .AnyAsync(a =>
         a.AppointmentDate==normalized &&(a.Status== "Accepted"|| a.Status == "Pending")
         );*/
            var doctorConflict = await _context.Appointments
    .AnyAsync(a =>
        a.DoctorId == request.DoctorId &&
        a.AppointmentDate == normalized &&
        (a.Status == "Accepted" || a.Status == "Pending")
    );
            var patientConflict = await _context.Appointments
    .AnyAsync(a =>
        a.PatientId == request.PatientId &&
        a.AppointmentDate == normalized &&
        (a.Status == "Accepted" || a.Status == "Pending")
    );
            if (doctorConflict)
                return new AppointmentResponseDTO
                {
                    Success = false,
                    StatusMessage = "Doctor already has an appointment at this time"
                };

            if (patientConflict)
                return new AppointmentResponseDTO
                {

                    Success = false,
                    StatusMessage = "Patient already has an appointment at this time"
                };
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
                AppointmentDate = request.AppointmentDate,
                Status = "Pending"

            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            //send notification to doctor and patient (optional)
            //
            _notificationService.SendAsync(appointment.Patient.UserId, "Booking Request", $"New appointment request from you on {request.AppointmentDate}", "dsad");

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
            Success=true,
           // PatientName = await _patientService.GetPatientNameAsync(a.PatientId) ?? "Unknown",
           // DoctorName = await _context.Doctors.Where(d => d.DoctorId == a.DoctorId).Select(d => d.FullName).FirstOrDefaultAsync(),
            AppointmentDate = a.AppointmentDate
           
        };
    }
}
