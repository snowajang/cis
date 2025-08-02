using CISApps.Models.Linkage.Department.Move;

namespace CISApps.Models.Linkage.Department
{
    public class HouseDetail
    {
        public List<House> house { set; get; } = new();
        public int totalHouse { set; get; }
    }
}
