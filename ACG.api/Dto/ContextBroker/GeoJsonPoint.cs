using System;
using FIWARE.ContextBroker.Interfaces;

namespace ACG.api.Dto.ContextBroker
{
    public class GeoJsonPoint : IGeoJSON
    {
        public string Type { get; set; } = "Point";
        public double[] Coordinates { get; set; }
    }
}
