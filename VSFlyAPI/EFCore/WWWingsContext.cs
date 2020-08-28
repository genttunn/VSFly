using System;
using System.Collections.Generic;
using System.Text;
using EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class WWWingsContext : DbContext
    {
        public DbSet<Flight> FlightSet { get; set; }
        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<Booking> BookingSet { get; set; }

        //public WWWingsContext() { }
       
        //for dep injection
        public WWWingsContext(DbContextOptions<WWWingsContext> options)
        : base(options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // configured in VSFlyAPI.Startup through dep injection
            //string ConnectionString = @"Server=(localDB)\MSSQLLocalDB;Database=WWWings_2020Step1;Trusted_Connection=True;MultipleActiveResultSets=True;App=EFCore";
            //builder.UseSqlServer(ConnectionString);

            // lazy loading
            builder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // composed
            builder.Entity<Booking>().HasKey(x => new { x.FlightID, x.CustomerID });

            // mapping many to many relationship
            builder.Entity<Booking>()
                .HasOne(x => x.Flight)
                .WithMany(x => x.BookingSet)
                .HasForeignKey(x => x.FlightID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.BookingSet)
                .HasForeignKey(x => x.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
