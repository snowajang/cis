using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CISApps.Models.Rest
{
    public static class ObjExtension
    {
        public static T? ToObjectJson<T>(this object str) where T : class {
            string json = "{}";
			json = str?.ToString() ?? "{}";
			try
            {
                json = json.Replace("\\", "");
                T? t = JsonSerializer.Deserialize<T>(json) ;
                return t;
            } catch (Exception ex)
            {
                Console.WriteLine(str);
                Console.WriteLine(ex.Message);
                return null;
            }
		}

        public static string ToJsonString(this object str)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.Thai, UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true                
            };
            return JsonSerializer.Serialize(str, options);
        }

        public static async Task<T> getJsonObjectAsync<T>(this HttpResponseMessage res)
        {
            try
            {
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    T? jobj = JsonSerializer.Deserialize<T>(json)!;
                    return jobj;
                }
                else
                {
                    return default(T)!;
                }
            }
            catch
            {
                return default(T)!;
            }
        }
    }
}
