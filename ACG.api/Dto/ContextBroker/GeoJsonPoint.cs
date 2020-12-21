using System;
using FIWARE.ContextBroker.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ACG.api.Dto.ContextBroker
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GeoJsonPoint : IGeoJSON
    {
        public string Type { get; set; } = "Point";
        public double[] Coordinates { get; set; }
    }
}
