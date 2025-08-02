namespace CISApps.Models.Linkage.Water
{
    public class WaterRegionService
    {
        public string address { get; set; } = "";
        public string amount { get; set; } = "";
        public string barcode { get; set; } = "";
        public string custcode { get; set; } = "";
        public string custname { get; set; } = "";
        public string docdate { get; set; } = "";
        public string docno { get; set; } = "";
        public string duedate { get; set; } = "";
        public string unit { get; set; } = "";
        public Use12m use12m { get; set; } = new();
        public string vat { get; set; } = "";
    }

    public class Use12m
    {
        //public string 1 { set; get;} = "";
    }
}
