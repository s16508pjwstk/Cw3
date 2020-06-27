using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.GeneratedModels
{
    public class DoctorDbContext : DbContext
    {

        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


        public DoctorDbContext()
        {

        }

        public DoctorDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// FluentAPI
            //modelBuilder.Entity<Studies>()
            //    .HasKey(e => e.IdStudies); //[Key]

            //modelBuilder.Entity<Studies>()
            //    .Property(e => e.Name)
            //    .HasMaxLength(100) // [MaxLength(100)]
            //    .IsRequired();

            var dictDoctors = new List<Doctor>();
            dictDoctors.Add(new Doctor
            {
                IdDoctor = 1,
                FirstName = "Jurgen",
                LastName = "Thorwald",
                Email = "xxx@xxx.pl"
            });

            dictDoctors.Add(new Doctor
            {
                IdDoctor = 2,
                FirstName = "House",
                LastName = "Houser",
                Email = "xxx@xxx.pl"
            });

            modelBuilder.Entity<Doctor>()
                .HasData(dictDoctors.ToArray());

            var dictPatients = new List<Patient>();
            dictPatients.Add(new Patient
            {
                IdPatient = 1,
                FirstName = "Ewa",
                LastName = "Godlewska",
                BirthDate = DateTime.Now,
            });

            dictPatients.Add(new Patient
            {
                IdPatient = 2,
                FirstName = "Janek",
                LastName = "Grzelak",
                BirthDate = DateTime.Now,
            });

            modelBuilder.Entity<Patient>()
                .HasData(dictPatients.ToArray());


            // to unlock

            var dictPrescriptions = new List<Prescription>();
            dictPrescriptions.Add(new Prescription
            {
                IdPrescription = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now,
                IdPatient = 1,
                IdDoctor = 1
            });

            dictPrescriptions.Add(new Prescription
            {
                IdPrescription = 2,
                Date = DateTime.Now,
                DueDate = DateTime.Now,
                IdPatient = 2,
                IdDoctor = 2
            });

            modelBuilder.Entity<Prescription>()
                .HasData(dictPrescriptions.ToArray());
        }
    }
}
