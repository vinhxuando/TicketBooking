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
    
    public partial class Flight
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Flight()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public string ID { get; set; }
        public int AirlineID { get; set; }
        public int PlaneID { get; set; }
        public int StatusID { get; set; }
        public System.DateTime DepartTime { get; set; }
        public System.DateTime DestTime { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public int CurReg { get; set; }
    
        public virtual AirlineCompany AirlineCompany { get; set; }
        public virtual Plane Plane { get; set; }
        public virtual StatusBoard StatusBoard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
