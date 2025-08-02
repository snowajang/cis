using static CISApps.Models.Linkage.Department.MoveIn;

namespace CISApps.Models.Linkage.Department.Move
{
    public class Own
    {
        public long personalID { get; set; }
        public string titleDesc { get; set; } = "";
        public string firstName { get; set; } = "";
        public string middleName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string nationalityDesc { get; set; } = "";
        public string gender { get; set; } = "";
        public int dateOfBirth { get; set; }
        public Father? father { get; set; }
        public Mother? mother { get; set; }
    }
}
