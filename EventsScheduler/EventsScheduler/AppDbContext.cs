﻿using EventsScheduler.Entities;
using System.Data.Entity;

namespace EventsScheduler
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base("PostgreSQLConnection") {}

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // PostgreSQL uses the public schema by default - not dbo.
            modelBuilder.HasDefaultSchema("public");
            
            modelBuilder.Entity<User>()
                        .HasMany(u => u.Events)
                        .WithMany(ev => ev.Participants)
                        .Map(uEv =>
                        {
                            uEv.MapLeftKey("UserId");
                            uEv.MapRightKey("CourseId");
                            uEv.ToTable("UsersEvents");
                        });

            modelBuilder.Entity<Event>()
                        .HasRequired(ev => ev.Creator)
                        .WithMany(u => u.CreatedEvents);

            base.OnModelCreating(modelBuilder);
        }
    }
}
