using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky_Pet_Clinic2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lucky_Pet_Clinic2.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Clients> Clients { get; set; }
        public DbSet<Pets> Pets { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<Surgery> Surgeries { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Vets> Vets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationship between Clients and Pets
            modelBuilder.Entity<Pets>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.ClientId);

            // Configure relationship between Pets and Examinations
            modelBuilder.Entity<Examination>()
                .HasOne(e => e.Pet)
                .WithMany(p => p.Examinations)
                .HasForeignKey(e => e.PetId);

            // Configure relationship between Pets and Surgeries
            modelBuilder.Entity<Surgery>()
                .HasOne(s => s.Pet)
                .WithMany(p => p.Surgeries)
                .HasForeignKey(s => s.PetId);

            // Configure relationship between Pets and Vaccinations
            modelBuilder.Entity<Vaccination>()
                .HasOne(v => v.Pet)
                .WithMany(p => p.Vaccinations)
                .HasForeignKey(v => v.PetId);
        }
    }
}
