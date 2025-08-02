namespace CISApps.Models.Linkage.Department.Cards
{
    public class CardBP1
    {
        public virtual string documentNumber { get; set; } = "";
        public virtual string firstName { get; set; } = "";
        public virtual string lastName { get; set; } = "";
        public virtual int issueDate { get; set; }

        public virtual Card card { get; set;} = new Card();
        public virtual CardImage cardImage { get; set;} = new CardImage();
    }
}
