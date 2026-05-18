using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicsAPP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>().ToTable("Doctors");

            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            modelBuilder.Entity<Feedback>().ToTable("Feedbacks");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Feedback>()
    .HasOne(f => f.Doctor)
    .WithMany(d => d.Feedbacks)
    .HasForeignKey(f => f.DoctorId)
    .OnDelete(DeleteBehavior.NoAction);
          /*  modelBuilder.Entity<Doctor>()
                 .HasOne(d => d.User)
                 .WithMany(u => u.Doctors)
                 .HasForeignKey(d => d.UserId)
                 .OnDelete(DeleteBehavior.Restrict);*/

             /* modelBuilder.Entity<Appointment>()
                  .HasOne(a => a.Patient)
                  .WithMany(p => p.Appointments)
                  .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);*/

              modelBuilder.Entity<Appointment>()
                  .HasOne(a => a.Doctor)
                  .WithMany(d => d.Appointments)
                  .HasForeignKey(a => a.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);

              modelBuilder.Entity<Feedback>()
                  .HasOne(f => f.Appointment)
                  .WithMany(a => a.Feedbacks)
                  .HasForeignKey(f => f.AppointmentId)
                  .OnDelete(DeleteBehavior.Restrict);

             /* modelBuilder.Entity<Feedback>()
                  .HasOne(f => f.Patient)
                  .WithMany(p => p.Feedbacks)
                  .HasForeignKey(f => f.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);*/

              modelBuilder.Entity<Feedback>()
                  .HasOne(f => f.Doctor)
                  .WithMany(d => d.Feedbacks)
                  .HasForeignKey(f => f.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
