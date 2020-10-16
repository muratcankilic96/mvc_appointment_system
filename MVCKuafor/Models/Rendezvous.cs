namespace MVCKuafor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rendezvous
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RendezvousId { get; set; }

        public DateTime TimeSlot { get; set; }

        [Required(ErrorMessage = "Employee Id is not available.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Customer Id is not available.")]
        public int CustomerId { get; set; }

        public byte IsUnavailable { get; set; }      
    }
}
