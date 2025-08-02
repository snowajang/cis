namespace CISApps.Models.Linkage.Department.Move
{
    public class Move
    {
        public int type { get; set; }
        public string description { get; set; } = "-";
        public string petitionNo { get; set; } = "-";
        public string documentNo { get; set; } = "-";
        public House? house { get; set; }
        public Own? own { get; set; }
        public Inform? inform { get; set; }
        public int terminateDate { get; set; }
        public int oldDateMoveIn { get; set; }
    }
}
