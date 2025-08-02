using CISApps.Models.Api;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Borderpass;
using CISApps.Models.Linkage.Dea;
using CISApps.Models.Linkage.Department;
using CISApps.Models.Linkage.Dsi;
using CISApps.Models.Linkage.Electric;
using CISApps.Models.Linkage.Gun;
using CISApps.Models.Linkage.Insurance;
using CISApps.Models.Linkage.Nsho;
using CISApps.Models.Linkage.Oncb;
using CISApps.Models.Linkage.Police;
using CISApps.Models.Linkage.Water;
using CISApps.Models.Rest;
using System.Security.Cryptography;

namespace CISApps.Models
{
    public class Searchs : ErrorLinkage
    {
        public Privilate? privilate { get; set; } 
        public People? people { get; set; }

        public void setData(IFormCollection f)
        {
			try
			{
				this.people = new People();
				this.people.pop = f["pop"].ToString().ToObjectJson<Pop>();
				this.people.house = f["house"].ToObjectJson<House>();
				this.people.card = f["card"].ToObjectJson<Card>();
				this.people.cardImage = f["cardImage"].ToObjectJson<CardImage>();
				this.people.cardDetail = f["cardDetail"].ToObjectJson<CardDetail>();
				this.people.cardDetailImage = f["cardDetailImage"].ToObjectJson<CardImage>();
				if (this.people.cardDetail!=null)
				{
					int vix = -1;
					for (vix = 0; vix < this.people?.cardDetail?.cardbp?.Count; vix++) {
						var bp1 = this.people.cardDetail.cardbp[vix];
						if (bp1 != null && bp1.documentNumber == this.people?.card?.documentNumber)
						{
							bp1.card = this.people.card;
							if (this.people?.cardDetailImage == null)
							{
                                bp1.cardImage = this.people?.cardImage ?? new();
                            } else if (this.people?.cardDetailImage != null)
							{
								bp1.cardImage = this.people?.cardDetailImage ?? new();
                            }
							
							break;
                        }
                    }
                }
                this.people.moveIn = f["moveIn"].ToObjectJson<MoveIn>();
				this.people.moveOut = f["moveOut"].ToObjectJson<MoveOut>();
				this.people.rename = f["rename"].ToObjectJson<Rename>();
				this.people.marry = f["marry"].ToObjectJson<Marry>();
				this.people.divorce = f["divorce"].ToObjectJson<Divorce>();
				this.people.birth = f["birth"].ToObjectJson<Birth>();
				this.people.death = f["death"].ToObjectJson<Death>();
				this.people.child = f["child"].ToObjectJson<Child>();
				this.people.oncb = f["oncb"].ToObjectJson<OncbService>();
				this.people.police = f["police"].ToObjectJson<PoliceService>();
				this.people.borderpass = f["borderpass"].ToObjectJson<BorderPassService>();
				this.people.eleccenter = f["eleccenter"].ToObjectJson<ElecCenterService>();
				this.people.elecregion = f["elecregion"].ToObjectJson<ElecRegionService>();
				this.people.watercenter = f["watercenter"].ToObjectJson<WaterCenterService>();
				this.people.waterregion = f["waterregion"].ToObjectJson<WaterRegionService>();
				this.people.nsho = f["nsho"].ToObjectJson<NshoService>();
				this.people.guncard = f["guncard"].ToObjectJson<GunCardService>();
				this.people.insurance = f["insurance"].ToObjectJson<InsuranceService>();
                this.people.housedetail = f["housedetail"].ToObjectJson<HouseDetail>();
                this.people.popinhouse = f["popinhouse"].ToObjectJson<PopInHouse>();
                this.people.alienpop = f["alienpop"].ToObjectJson<AlienPop>();
                this.people.aliencard = f["aliencard"].ToObjectJson<AlienCard>();
                this.people.alienimage = f["alienimage"].ToObjectJson<AlienImage>();
                this.people.dsi = f["dsi"].ToObjectJson<DsiService>();
				this.people.transport = new() { 
					Property1 = f["transport"].ToObjectJson<List<Linkage.Transport.Class1>>() ?? new()
				}; 
                this.people.transportcard = new()
                {
                    Property1 = f["transportcard"].ToObjectJson<List<Linkage.Transport.Tcard>>() ?? new()
                }; 
                this.people.education = f["education"].ToObjectJson<Linkage.Education.EducationService>();
            }
			catch (Exception ex)
			{
				throw ex ?? new Exception("Data Error");
			}
		}
    }
}
