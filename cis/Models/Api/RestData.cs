namespace CISApps.Models.Api
{
    public class RestData<T>
    {
        public List<T>? data { get; set; }
        public Error? error { get; set; }
    }
}
