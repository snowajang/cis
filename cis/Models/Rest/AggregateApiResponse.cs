
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace cis.Models.Rest
{
    public class AggregateApiResponse
    {
        public string? errorMessage { get; set; }
        public int errorNumber { get; set; }
        public List<AggregateApiItem> data { get; set; } = new();
        public int executeTimeMs { get; set; }
    }

    public sealed class AggregateApiItem
    {
        public int serviceID { get; set; }
        public int responseStatus { get; set; }
        public string responseContentType { get; set; } = string.Empty;
        public JsonElement responseData { get; set; }
        public JsonElement? responseError { get; set; }
        public int responseTimeMs { get; set; }
    }
}