namespace CISApps.Models.Linkage.Electric
{
    public class ElecCenterService
    {
        public List<Customerinfo> customerInfo { get; set; } = new();
        public bool empty { get; set; }
        public bool error { get; set; }
        public string id { get; set; } = "";
        public int rowCount { get; set; }
        public string status { get; set; } = "";
        public string statusDetail { get; set; } = "";
    }

    public class Customerinfo
    {
        public string contractNumber { get; set; } = "";
        public string country { get; set; } = "";
        public string district { get; set; } = "";
        public string floor { get; set; } = "";
        public string houseNum1 { get; set; } = "";
        public string houseNum2 { get; set; } = "";
        public string houseSub { get; set; } = "";
        public string installationNumber { get; set; } = "";
        public string name1 { get; set; } = "";
        public string name2 { get; set; } = "";
        public string name3 { get; set; } = "";
        public string postalCode { get; set; } = "";
        public string province { get; set; } = "";
        public string provinceCode { get; set; } = "";
        public string soi { get; set; } = "";
        public string street { get; set; } = "";
        public string subDistrict { get; set; } = "";
        public string title { get; set; } = "";
        public List<Usagehistory> usageHistory { get; set; } = new();
        public string village { get; set; } = "";
    }

    public class Usagehistory
    {
        public float billAmount { get; set; }
        public string billDate { get; set; } = "";
        public int kwh { get; set; }
    }

}
