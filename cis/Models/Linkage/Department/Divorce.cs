namespace CISApps.Models.Linkage.Department
{
    public class Divorce
    {
        public int total { get; set; }
        public List<Alldiv> allDiv { get; set; } = new List<Alldiv>();
    }

    public class Alldiv
    {
        public int divorceDate { get; set; }
        public string divorcePlace { get; set; } = "";
        public string divorcePlaceDesc { get; set; } = "";
        public string divorcePlaceProvince { get; set; } = "";
        public string divorceType { get; set; } = "";
        public long divorceID { get; set; }
        public int divorceTime { get; set; }
        public int marryDate { get; set; }
        public string marryPlace { get; set; } = "";
        public string marryPlaceDesc { get; set; } = "";
        public string marryPlaceProvince { get; set; } = "";
        public string marryType { get; set; } = "";
        public int marryID { get; set; }
        public int marryTime { get; set; }
        public long malePID { get; set; }
        public string maleOtherDocID { get; set; } = "";
        public int maleTitleCode { get; set; }
        public string maleTitleDesc { get; set; } = "";
        public string maleFirstName { get; set; } = "";
        public string maleMiddleName { get; set; } = "";
        public string maleLastName { get; set; } = "";
        public int maleDateOfBirth { get; set; }
        public int maleAge { get; set; }
        public int maleNationalityCode { get; set; }
        public string maleNationalityDesc { get; set; } = "";
        public string maleFullnameAndRank { get; set; } = "";
        public long femalePID { get; set; }
        public string femaleOtherDocID { get; set; } = "";
        public int femaleTitleCode { get; set; }
        public string femaleTitleDesc { get; set; } = "";
        public string femaleFirstName { get; set; } = "";
        public string femaleMiddleName { get; set; } = "";
        public string femaleLastName { get; set; } = "";
        public int femaleDateOfBirth { get; set; }
        public int femaleAge { get; set; }
        public int femaleNationalityCode { get; set; }
        public string femaleNationalityDesc { get; set; } = "";
        public string femaleFullnameAndRank { get; set; } = "";
    }
    
}
