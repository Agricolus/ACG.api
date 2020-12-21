using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace ACG.api.Model
{
    [Table("fields")]
    public class Field
    {        
        public Guid Id { get; set; }
        [StringLength(64)]
        public string ExternalId { get; set; }
        [StringLength(64)]
        public string UserId { get; set; }
        [StringLength(64)]
        public string ProducerCode { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        public double Area { get; set; }
        public string ClientId { get; set; }
        public MultiPolygon Boundaries { get; set; }
        public MultiPolygon UnpassableBoundaries { get; set; }
        public DateTime ModificationTime { get; set; }
        public bool IsRegistered { get; set; }
    }
}
