namespace CISApps.Models.Linkage.Transport
{
    public class TransportService
    {
        public List<Class1> Property1 { get; set; } = new();
    }

    public class Class1
    {
        public Getvehicleinfooutput getVehicleInfoOutput { get; set; } = new();
    }

    public class Getvehicleinfooutput
    {
        public string regDate { get; set; } = "";
        public string plate1 { get; set; } = "";
        public string plate2 { get; set; } = "";
        public string offLocCode { get; set; } = "";
        public string vehTypeDesc { get; set; } = "";
        public string kindDesc { get; set; } = "";
        public string brnDesc { get; set; } = "";
        public string modelName { get; set; } = "";
        public string numBody { get; set; } = "";
        public string expDate { get; set; } = "";
        public string engBrnDesc { get; set; } = "";
        public string numEng { get; set; } = "";
        public string fuelDesc { get; set; } = "";
        public string cly { get; set; } = "";
        public string cc { get; set; } = "";
        public string wgtCar { get; set; } = "";
        public string carStatus { get; set; } = "";
        public string holdFlag { get; set; } = "";
        public string docNo1 { get; set; } = "";
        public string owner1 { get; set; } = "";
        public string addressOwner1 { get; set; } = "";
        public string docNo2 { get; set; } = "";
        public string owner2 { get; set; } = "";
        public string addressOwner2 { get; set; } = "";
        public List<Carchkmascolorlist> carChkMasColorList { get; set; } = new();
    }

    public class Carchkmascolorlist
    {
        public Carchkmascolor carChkMasColor { get; set; } = new();
    }

    public class Carchkmascolor
    {
        public string colorDesc { get; set; } = "";
    }

}
