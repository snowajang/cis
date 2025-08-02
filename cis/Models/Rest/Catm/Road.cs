namespace CISApps.Models.Rest.Catm
{
    public class Road
    {
        public string rcode { get; set; } = "";
        public int cc_code { get; set; }
        public int aa_code { get; set; }
        public int tt_code { get; set; }
        public List<DataOut> data { get; set; } = new List<DataOut>();
    }
}
