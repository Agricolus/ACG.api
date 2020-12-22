using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace ACG.api.Model
{
    [Table("machines_history")]
    public class MachineHistory
    {
        public Guid Id { get; set; }

        [StringLength(64)]
        public Guid MachineId { get; set; }
       
        public double? Lat { get; set; }
        
        public double? Lng { get; set; }
        
        public DateTime? PTime { get; set; }

        public Point Position { get; set; }
    }
}
