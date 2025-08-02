namespace CISApps.Models.Linkage.Department.Move
{
    public class House
    {
        public long houseID { get; set; }
        public int moveDate { get; set; }
        public string houseOwner { get; set; } = "";
        public Address address { get; set; } = new();
    }
}
