namespace CISApps.Models.Api
{ 
    public class CheckUserLinkage
    {
        public string desc { get; set; } = "";
        public Lkuser lkuser { get; set; } = new();
        public string status { get; set; } = "";
    }

    public class Lkuser
    {
        public string desc { get; set; } = "";
        public List< Office> office { get; set; } = new();
        public long pid { get; set; }
        public string status { get; set; } = "";
    }

    public class Office
    {
        public int office_id { get; set; }
        public string office_name { get; set; } = "";
    }
}
