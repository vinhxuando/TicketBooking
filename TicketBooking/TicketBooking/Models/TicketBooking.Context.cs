﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketBooking.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TicketBookingEntities : DbContext
    {
        public TicketBookingEntities()
            : base("name=TicketBookingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AirlineCompany> AirlineCompanies { get; set; }
        public virtual DbSet<BasicPrice> BasicPrices { get; set; }
        public virtual DbSet<CardInfo> CardInfoes { get; set; }
        public virtual DbSet<CardProvider> CardProviders { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<GradeTable> GradeTables { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Plane> Planes { get; set; }
        public virtual DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SeatDetail> SeatDetails { get; set; }
        public virtual DbSet<StatusBoard> StatusBoards { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
    }
}
