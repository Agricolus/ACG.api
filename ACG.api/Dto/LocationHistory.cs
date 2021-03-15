using System;
using System.Collections.Generic;

namespace ACG.api.Dto
{
    public class LocationHistory
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Operation { get; set; }
        public List<TimePoint> Points { get; set; }
    }

    public class TimePoint {
        public TimeSpan Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double? Z { get; set; }
    }
}
