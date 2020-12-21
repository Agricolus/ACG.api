using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACG.api.Model
{
    [Table("clients")]
    public class Client
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
        public bool IsRegistered { get; set; }
    }

}
