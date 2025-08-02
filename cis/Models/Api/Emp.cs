namespace CISApps.Models.Api
{
    public class Emp
    {
        public long pid { get; set; }
        public int termdate { get; set; }
        public string title { get; set; } = string.Empty;
        public string fname { get; set; } = string.Empty;
        public string mname { get; set; } = string.Empty;
        public string lname { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int privilateid { get; set; }
        public int adminid { get; set; }
        public int userid { get; set; }

		public string cid { get; set; } = "";
		public string sid { get; set; } = "";
		public string tkey32 { get; set; } = "";

        public int accessdate { get; set; }
        public int samepid { get; set; }
        public string host { get; set; } = "";
        public int port { get; set; }
        public string office { get; set; } = "";
    }
}
