namespace CISApps.Models.Linkage.Gun
{
    public class GunCardService
    {
        public List<GunCards> allName { get; set; } = new List<GunCards>();
        public int processTimestamp { get; set; }
        public string remark { get; set; } = "";
        public int total { get; set; }
    }

    public class GunCards {
        public string amphorDesc { get; set; } = "";
        public string amphorDesc2 { get; set; } = "";
        public int applicantType { get; set; }
        public string businessName { get; set; } = "";
        public string businessType { get; set; } = "";
        public string districtDesc { get; set; } = "";
        public string districtDesc2 { get; set; } = "";
        public int docDate { get; set; }
        public string docID { get; set; } = "";
        public string docPlace { get; set; } = "";
        public string docPlaceDesc { get; set; } = "";
        public string docPlaceProvince { get; set; } = "";
        public int expireDate { get; set; }
        public string firstName { get; set; } = "";
        public string firstName2 { get; set; } = "";
        public string fullNameAndRank { get; set; } = "";
        public string fullNameAndRank2 { get; set; } = "";
        public int genderCode { get; set; }
        public int genderCode2 { get; set; }
        public string genderDesc { get; set; } = "";
        public string genderDesc2 { get; set; } = "";
        public string gunCharacteristic { get; set; } = "";
        public string gunProduct { get; set; } = "";
        public string gunRegistrationId { get; set; } = "";
        public string gunSerialNo { get; set; } = "";
        public string gunSize { get; set; } = "";
        public string gunType { get; set; } = "";
        public int hid { get; set; }
        public int hid2 { get; set; }
        public string hidRcodeCode { get; set; } = "";
        public string hidRcodeCode2 { get; set; } = "";
        public string hidRcodeDesc { get; set; } = "";
        public string hidRcodeDesc2 { get; set; } = "";
        public string hno { get; set; } = "";
        public string hno2 { get; set; } = "";
        public string lastName { get; set; } = "";
        public string lastName2 { get; set; } = "";
        public string middleName { get; set; } = "";
        public string middleName2 { get; set; } = "";
        public long personalId { get; set; }
        public long personalId2 { get; set; }
        public string provinceDesc { get; set; } = "";
        public string provinceDesc2 { get; set; } = "";
        public string signFullName { get; set; } = "";
        public string signTitleDesc { get; set; } = "";
        public string soi { get; set; } = "";
        public string soi2 { get; set; } = "";
        public string thanon { get; set; } = "";
        public string thanon2 { get; set; } = "";
        public int titleCode { get; set; }
        public int titleCode2 { get; set; }
        public string titleDesc { get; set; } = "";
        public string titleDesc2 { get; set; } = "";
        public string trok { get; set; } = "";
        public string trok2 { get; set; } = "";
    }
}
