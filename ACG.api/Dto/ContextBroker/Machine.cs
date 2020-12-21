using System;
using System.ComponentModel.DataAnnotations.Schema;
using FIWARE.ContextBroker.Interfaces;
using FIWARE.ContextBroker.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ACG.api.Dto.ContextBroker
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Machine : IContextBrokerEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ExternalId { get; set; }
        public string UserId { get; set; }
        public string ProducerCode { get; set; }
        public GeoJsonPoint Position { get; set; }

        [JsonConverter(typeof(UriEscapeJsonConverter), new char[] { '<', '>', '"', '\'', '=', ';', '(', ')' })]
        public string Name { get; set; }

        public string Model { get; set; }
        public string Code { get; set; }

        [JsonConverter(typeof(UriEscapeJsonConverter), new char[] { '<', '>', '"', '\'', '=', ';', '(', ')' })]
        public string Description { get; set; }

        public string ProducerCommercialName { get; set; }
        // [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime PTime { get; set; }
    }
}
