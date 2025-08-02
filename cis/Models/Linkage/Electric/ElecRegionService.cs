namespace CISApps.Models.Linkage.Electric
{
    public class ElecRegionService
    {
        public List<Calist>? CAList { get; set; } = new();
        public string status { get; set; } = "";
        public string rowCount { get; set; } = "";
    }

    public class Calist
    {
        public List<Billlist>? BillList { get; set; } = new();
        public string Name { get; set; } = "";
        public string CANo { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string HomeNo { get; set; } = "";
        public string Moo { get; set; } = "";
        public string VillageName { get; set; } = "";
        public string floor { get; set; } = "";
        public string Room { get; set; } = "";
        public string Street { get; set; } = "";
        public string SubDistrict { get; set; } = "";
        public string Distict { get; set; } = "";
        public string Province { get; set; } = "";
        public string PostCode { get; set; } = "";
        public string Class { get; set; } = "";
        public string MeterSize { get; set; } = "";
        public string PEAName { get; set; } = "";
        public string RegisDate { get; set; } = "";
    }

    public class Billlist
    {
        public DateTime RecordDate { get; set; }
        public string InvoiceNo { get; set; } = "";
        public string BillPeriod { get; set; } = "";
        public float Unit { get; set; }
        public float Cost { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
