namespace CISApps.Models.Rest.Catm
{
    public class CItem
    {
        public int dataCount { get; set; }
        public string error { get; set; } = "";

        public List<DataOut> dataItem { get; set; } = new List<DataOut>();
    }
}
