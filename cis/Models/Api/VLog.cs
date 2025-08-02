namespace CISApps.Models.Api
{
    public class VLog
    {
        public int logdate { get; set; }
        public int logtime { get; set; }
        public long pid { get; set; }
        public string desc { get; set; } = "";

    }
}
