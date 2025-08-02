namespace CISApps.Models.Api
{
    public class Error
    {
        public int code { get; set; }
        public string msg { get; set; } = "";

        public bool fail { get; set; }

	}
}
