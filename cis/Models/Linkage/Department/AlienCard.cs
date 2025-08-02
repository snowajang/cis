namespace CISApps.Models.Linkage.Department
{
    public class AlienCard
    {
        public AlAddress address { get; set; } = new AlAddress();
        public int titleCode { get; set; }
        public string titleDesc { get; set; } = "";
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string englishName { get; set; } = "";
        public int dateOfBirth { get; set; }
        public int genderCode { get; set; }
        public string genderText { get; set; } = "";
        public int nationalityCode { get; set; }
        public string nationalityDesc { get; set; } = "";
        public int cardIssueDate { get; set; }
        public int cardExpireDate { get; set; }
        public string entrepreneurId { get; set; } = "";
        public Entrepreneuraddress entrepreneurAddress { get; set; } = new Entrepreneuraddress();
        public string entrepreneurName { get; set; } = "";
        public int workPermitIssueDate { get; set; }
        public int workPerminExpireDate { get; set; }
        public int occupationTypeCode { get; set; }
        public string occupationTypeDesc { get; set; } = "";
        public int occupationCode { get; set; }
        public string occupationDesc { get; set; } = "";
        public string healthCareProvider { get; set; } = "";
        public string healthCareResult { get; set; } = "";
        public string workPlaceDescription { get; set; } = "";
        public Workplaceaddress workPlaceAddress { get; set; } = new Workplaceaddress();
    }

    public class AlAddress
    {
        public long houseID { get; set; }
        public string houseNo { get; set; } = "";
        public int villageNo { get; set; }
        public string alleyWayDesc { get; set; } = "";
        public string alleyDesc { get; set; } = "";
        public string roadDesc { get; set; } = "";    
        public string subdistrictDesc { get; set; } = "";     
        public string districtDesc { get; set; } = "";
        public string provinceDesc { get; set; } = "";
        public int ccaattmm { get; set; }
    }

    public class Entrepreneuraddress
    {
        public long houseID { get; set; }
        public string houseNo { get; set; } = "";
        public int villageNo { get; set; }
        public string alleyWayDesc { get; set; } = "";
        public string alleyDesc { get; set; } = "";    
        public string roadDesc { get; set; } = "";
        public string subdistrictDesc { get; set; } = "";
        public string districtDesc { get; set; } = "";
        public string provinceDesc { get; set; } = "";
        public int ccaattmm { get; set; }
    }

    public class Workplaceaddress
    {
        public long houseID { get; set; }
        public string houseNo { get; set; } = "";
        public int villageNo { get; set; }
        public string alleyWayDesc { get; set; } = "";
        public string alleyDesc { get; set; } = "";
        public string roadDesc { get; set; } = "";
        public string subdistrictDesc { get; set; } = "";
        public string districtDesc { get; set; } = "";
        public string provinceDesc { get; set; } = "";
        public int ccaattmm { get; set; }
    }

}
