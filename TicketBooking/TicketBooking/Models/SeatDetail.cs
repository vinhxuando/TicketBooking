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
    
    public partial class SeatDetail
    {
        public int PlaneID { get; set; }
        public string GradeID { get; set; }
        public int NumOfSeat { get; set; }
        public int CurReg { get; set; }
    
        public virtual GradeTable GradeTable { get; set; }
        public virtual Plane Plane { get; set; }
    }
}
