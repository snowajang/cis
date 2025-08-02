namespace CISApps.Models.Linkage.Transport
{
    public class TransportCardService
    {
        public List<Tcard> Property1 { get; set; } = new();
    }

    public class Tcard
    {
        public Driverlicenceinfo driverLicenceInfo { get; set; } = new();
    }

    public class Driverlicenceinfo
    {
        public string docNo { get; set; } = "";
        public string titleDesc { get; set; } = "";
        public string fName { get; set; } = "";
        public string lName { get; set; } = "";
        public string sex { get; set; } = "";
        public string addrNo { get; set; } = "";
        public string bldName { get; set; } = "";
        public string villageName { get; set; } = "";
        public string villageNo { get; set; } = "";
        public string soi { get; set; } = "";
        public string street { get; set; } = "";
        public string locDesc { get; set; } = "";
        public string zipCode { get; set; } = "";
        public string pltDesc { get; set; } = "";
        public string locFullDesc { get; set; } = "";
        public string conditionDesc { get; set; } = "";
        public string natDesc { get; set; } = "";
        public string pltNo { get; set; } = "";
        public string issDate { get; set; } = "";
        public string expDate { get; set; } = "";
        public string message { get; set; } = "";
    }

}
