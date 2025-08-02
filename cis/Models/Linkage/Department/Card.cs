using CISApps.Models.Linkage.Department.Cards;

namespace CISApps.Models.Linkage.Department
{
    public class Card
    {
        public Address address { get; set; } = new Address();
        public int birthDate { get; set; }
        public string blood { get; set; } = "";
        public string cancelCause { get; set; } = "";
        public string documentNumber { get; set; } = "";
        public string expireDate { get; set; } = "";
        public string foreignCountry { get; set; } = "";
        public string foreignCountryCity { get; set; } = "";
        public int issueDate { get; set; }
        public int issueTime { get; set; }
        public IName nameEN { get; set; } = new IName();
        public IName nameTH { get; set; } = new IName();
        public string phoneNumber { get; set; } = "";
        public string religion { get; set; } = "";
        public string religionOther { get; set; } = "";
        public string sex { get; set; } = "";
    }
}
