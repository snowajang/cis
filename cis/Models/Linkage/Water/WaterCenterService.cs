using System.ServiceModel;

namespace CISApps.Models.Linkage.Water
{
    public class WaterCenterService
    {
        public WResponse response { set; get; } = new();
    }

    public class WResponse
    {
        public WMessage message { set; get; } = new();
        public string status { set; get; } = "";
        public string currentDate { set; get; } = "";
    }

    public class WMessage
    {
        public string accountClassDescription { set; get; } = "";
        public string accountCode { set; get; } = "";
        public string accountName { set; get; } = "";
        public string branchCode { set; get; } = "";
        public string branchName { set; get; } = "";
        public string meterSizeDescription { set; get; } = "";
        public string numberOfDebtBill { set; get; } = "";

        public WAccountBillInfo accountBillInfo { set; get; } = new();
        public WSccountAddressInfo accountAddressInfo { set; get; } = new();
    }

    public class WAccountBillInfo
    {
        public int balanceGrossAmount { set; get; }
        public string billDate { set; get; } = "";
        public string billDueDate { set; get; } = "";
        public int consumption { set; get; }
        public int grossAmount { set; get; }
        public string paidDate { set; get; } = "";
        public int paidGrossAmount { set; get; }
        public string pmonth { set; get; } = "";
        public string pyear { set; get; } = "";
    }

    public class WSccountAddressInfo
    {
        public string address { set; get; } = "";
        public string houseId { set; get; } = "";
        public string houseNo { set; get; } = "";
    }

}