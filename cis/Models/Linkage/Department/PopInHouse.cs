namespace CISApps.Models.Linkage.Department
{
    public class PopInHouse
    {
        public List<Department.Houses.PersonalInHouse> personalInHouse { get; set; } = new();
        public int total { set; get; }
    }
}
