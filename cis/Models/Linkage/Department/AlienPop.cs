using CISApps.Models.Linkage.Department.Move;

namespace CISApps.Models.Linkage.Department
{
    public class AlienPop
    {
        public long houseID { get; set; }
        public string titleDesc { get; set; } = "";
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string genderDesc { get; set; } = "";
        public int dateOfBirth { get; set; }
        public int dateInThai { get; set; }
        public string nationalityDesc { get; set; } = "";
        public string bloodType { get; set; } = "";
        public string religion { get; set; } = "";
        public string marryStatus { get; set; } = "";
        public string spouseName { get; set; } = "";
        public string statusOfPersonDesc { get; set; } = "";
        public int personAddDate { get; set; }
        public int personUpdDate { get; set; }
        public string statusAdded { get; set; } = "";
        public int terminateDate { get; set; }
        public AlPop father { get; set; } = new AlPop();
        public AlPop mother { get; set; } = new AlPop();
        public Passport passport { get; set; } = new Passport();
        public Visa visa { get; set; } = new Visa();
        public string doeNumber { get; set; } = "";
    }

    public class AlPop
    {
        public int personalID { get; set; }
        public string name { get; set; } = "";
        public string nationalityDesc { get; set; } = "";
    }

    public class Passport
    {
        public string documentType { get; set; } = "";
        public string documentNo { get; set; } = "";
        public string documentIssuePlace { get; set; } = "";
        public int issueDate { get; set; }
        public int expireDate { get; set; }
    }

    public class Visa
    {
        public string documentNo { get; set; } = "";
        public int issueDate { get; set; }
        public int expireDate { get; set; }
        public string documentIssuePlace { get; set; } = "";
        public string visaType { get; set; } = "";
        public string visaRequestType { get; set; } = "";
    }

}
