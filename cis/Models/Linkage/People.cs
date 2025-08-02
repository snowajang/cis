namespace CISApps.Models.Linkage
{
    public class People
    {
        public virtual Department.Pop? pop { get; set; } // ข้อมูลบุคคล
        public virtual Department.House? house { get; set; } // ข้อมูลบ้าน
        public virtual Department.Card? card { get; set; } // ข้อมูลบัตร
        public virtual Department.CardImage? cardImage { get; set; } // ข้อมูลภาพใบหน้า
        public virtual Department.CardDetail? cardDetail { get; set; } // ประวัติการทำบัตร
        public virtual Department.CardImage? cardDetailImage { get; set; } // ข้อมูลภาพใบหน้า next
        public virtual Department.Rename? rename { get; set; } // ประวัติการเปลี่ยนชื่อ
        public virtual Department.MoveIn? moveIn { get; set; } // ประวัติการย้ายเข้า
        public virtual Department.MoveOut? moveOut { get;set; } // ประวัติการย้ายออก

        public virtual Department.Marry? marry { get; set; } // ประวัติการสมรส
        public virtual Department.Divorce? divorce { get; set; }// ประวัติการหย่า

        public virtual Department.Birth? birth { get; set; }// ข้อมูลการเกิด
        public virtual Department.Death? death { get; set; }// ข้อมูลการตาย

        public virtual Department.Child? child { get; set; }// ข้อมูลรายการบุตร

        // ปปส ตำรวจ ปปง ตรวจคนเข้าเมือง
        public virtual Oncb.OncbService? oncb { get; set; } // ปปส
        public virtual Police.PoliceService? police { get; set; } // ตำรวจ
        public virtual Borderpass.BorderPassService? borderpass { get; set; }  //ตรวจคนเข้าเมือง
        public virtual Dea.DeaService? dea { get; set; } // ปปง
        public virtual Dsi.DsiService? dsi { get; set; } // dsi

        // สาธารณูปโภค ไฟ น้า สิทธิรักษา ประกันสังคม
        public virtual Electric.ElecCenterService? eleccenter { get; set; } // ไฟฟ้านครหลวง
        public virtual Electric.ElecRegionService? elecregion { get; set; } // ไฟฟ้าส่วนภูมิภาค

        public virtual Water.WaterCenterService? watercenter { get; set; } // ประปานครหลวง
        public virtual Water.WaterRegionService? waterregion { get; set; } // ประปาส่วนภูมิภาค

        public virtual Nsho.NshoService? nsho { get; set; } // สิทธิรักษา

        public virtual Insurance.InsuranceService? insurance { get; set; } // ประกันสังคม

        public virtual Department.HouseDetail? housedetail {  get; set; } // คนในบ้าน 
        public virtual Department.PopInHouse? popinhouse { get; set; } // คนในบ้าน 

        public virtual Department.AlienPop? alienpop {  get; set; } // ข้อมูลแรงงาน

        public virtual Department.AlienCard? aliencard { get; set; } // ข้อมูลแรงงาน workpermit 

        public virtual Department.AlienImage? alienimage { get; set; } // ข้อมูลแรงงาน ภาพใบหน้า

        public virtual Gun.GunCardService? guncard { get; set; } // ข้อมูลใบอนุญาติพกพาปืน
		public virtual Gun.GunService? gun { get; set; } // ข้อมูลอาวุธปืน
		public virtual Transport.TransportService? transport { get; set; } // ข้อมูลยานพานะ
		public virtual Transport.TransportCardService? transportcard { get; set; } // ข้อมูลใบอนุญาตขับขี่
		public virtual Education.EducationService? education { get; set; } // ข้อมูลการศึกษา
	}
}
