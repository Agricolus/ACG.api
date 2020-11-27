using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACG.api.Model
{
    [Table("machines")]
    public class Machine
    {
        public Guid Id { get; set; }

        [StringLength(64)]
        public string ExternalId { get; set; }

        [StringLength(64)]
        public string UserId { get; set; }
        
        [StringLength(64)]
        public string ProducerCode { get; set; }
        
        public double? Lat { get; set; }
        
        public double? Lng { get; set; }
        
        [StringLength(64)]
        public string Name { get; set; }
        
        [StringLength(64)]
        public string Model { get; set; }
        
        [StringLength(64)]
        public string Code { get; set; }
        
        [StringLength(64)]
        public string Type { get; set; }
        
        [StringLength(128)]
        public string Description { get; set; }
        
        [StringLength(64)]
        public string ProducerCommercialName { get; set; }
        
        public DateTime? PTime { get; set; }                
        
        public String OtherData { get; set; }
    }
}
