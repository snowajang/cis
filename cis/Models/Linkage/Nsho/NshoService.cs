namespace CISApps.Models.Linkage.Nsho
{
    public class NshoService
    {
        public string CARDID { get; set; } = "";
        public string EXPDATE { get; set; } = "";
        public string HMAIN { get; set; } = "";
        public string HMAIN_NAME { get; set; } = "";
        public string HMAIN_OP { get; set; } = "";
        public string HMAIN_OP_NAME { get; set; } = "";
        public string HSUB { get; set; } = "";
        public string HSUB_NAME { get; set; } = "";
        public string MAININSCL { get; set; } = "";
        public string MAININSCL_NAME { get; set; } = "";
        public string MASTERCUP_ID { get; set; } = "";
        public NYdate NEW_DATE_REGISTER { get; set; } = new();
        public string NEW_EXPDATE { get; set; } = "";
        public string NEW_HMAIN { get; set; } = "";
        public string NEW_HMAIN_NAME { get; set; } = "";
        public string NEW_HMAIN_OP { get; set; } = "";
        public string NEW_HMAIN_OP_NAME { get; set; } = "";
        public string NEW_HSUB { get; set; } = "";
        public string NEW_HSUB_NAME { get; set; } = "";
        public string NEW_MAININSCL { get; set; } = "";
        public string NEW_MAININSCL_NAME { get; set; } = "";
        public string NEW_MASTERCUP_ID { get; set; } = "";
        public string NEW_PAID_MODEL { get; set; } = "";
        public string NEW_PURCHASEPROVINCE { get; set; } = "";
        public string NEW_PURCHASEPROVINCE_NAME { get; set; } = "";
        public string NEW_STAFFNAME { get; set; } = "";
        public string NEW_STARTDATE { get; set; } = "";
        public string NEW_SUBINSCL { get; set; } = "";
        public string NEW_SUBINSCL_NAME { get; set; } = "";
        public string NEW_TYPE_REGISTER { get; set; } = "";
        public string NEW_TYPE_REGISTER_DESC { get; set; } = "";
        public string PAID_MODEL { get; set; } = "";
        public string PERSON_ID { get; set; } = "";
        public string PURCHASEPROVINCE { get; set; } = "";
        public string PURCHASEPROVINCE_NAME { get; set; } = "";
        public string STARTDATE { get; set; } = "";
        public string SUBINSCL { get; set; } = "";
        public string SUBINSCL_NAME { get; set; } = "";
        public string WSID { get; set; } = "";
        public NYdate WS_DATETIME { get; set; } = new();
        public string WS_STATUS { get; set; } = "";

    }
    public class NYdate
    {
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int month { get; set; }
        public int second { get; set; }
        public int timezone { get; set; }
        public int year { get; set; }
    }

}
