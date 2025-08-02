namespace CISApps.Models.Linkage.Department
{

    public class Rename
    {
        public ChangeName firstName { get; set; } = new ChangeName();
        public ChangeName lastName { get; set; } = new ChangeName();
    }

    public class ChangeName
    {
        public string oldValue { get; set; } = "";
        public string newValue { get; set; } = "";
        public int dateOfChange { get; set; }
        public string rcodeCode { get; set; } = "";
        public string rcodeDesc { get; set; } = "";
    }
}
