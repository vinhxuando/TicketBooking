//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public string ID { get; set; }
        public string GradeID { get; set; }
        public string FlightID { get; set; }
        public int RegisteredUser_ID { get; set; }
        public double Price { get; set; }
    
        public virtual Flight Flight { get; set; }
        public virtual GradeTable GradeTable { get; set; }
        public virtual RegisteredUser RegisteredUser { get; set; }
    }
}
