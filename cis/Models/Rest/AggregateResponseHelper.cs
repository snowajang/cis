using System.Text.Json;

namespace cis.Models.Rest
{
    public static class AggregateResponseHelper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static bool HasData(AggregateApiResponse? response)
            => response?.data != null && response.data.Count > 0;

        public static AggregateApiItem? GetItem(AggregateApiResponse? response, int serviceId)
            => response?.data?.FirstOrDefault(x => x.serviceID == serviceId);

        public static T? DeserializeResponseData<T>(JsonElement responseData)
        {
            try
            {
                return responseData.Deserialize<T>(JsonOptions);
            }
            catch
            {
                return default;
            }
        }

        public static bool TryGetServiceResponse<T>(AggregateApiResponse? response, int serviceId, out T? result, out AggregateApiItem? item)
        {
            result = default;
            item = GetItem(response, serviceId);

            if (item == null)
                return false;

            if (item.responseData.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
                return false;

            result = DeserializeResponseData<T>(item.responseData);
            return result != null;
        }
    }
}
