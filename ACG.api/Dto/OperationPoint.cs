using System;

namespace ACG.api.Dto
{
    public class OperationPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double? Z { get; set; }
        public DateTime Timestamp { get; set; }
        public string Operation { get; set; }
    }
}
