using CISApps.Models.Api;
using CISApps.Models.Linkage.Department.Cards;
using CISApps.Models.Rest;
using CISApps.Models.Rest.Catm;
// using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CISApps.Models
{
    public class Tools
    {
        public static string[] months = {
            "", "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.",
            "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.",
            "ต.ค.", "พ.ย.", "ธ.ค."
        };

        public static string getDateTH(string? vdate)
        {
            int idate_int = 0;
            try
            {
                int.TryParse(vdate, out idate_int);
            }
            catch { }
            return getDateTH(idate_int);
        }
        public static string getDateTH(int? dob) {
            string idob = dob?.ToString() ?? "";
            if (idob.Length != 8)
            {
                return "-";
            }else if (idob.Equals("99999999"))
            {
                return "ตลอดชีพ";
            }
            var y = idob.Substring(0, 4);
            var m = "";
            var d = "";
            if (idob.Substring(4, 2) != "00")
            {
                m = " " + months[int.Parse(idob.Substring(4, 2))];
                if (idob.Substring(6, 2) != "00")
                {
                    d = " " + int.Parse(idob.Substring(6, 2)).ToString();
                }
            }

            return (d + m + " " + y);
        }
        public static string getTimeTH(string? vtime)
        {
            int itime_int = 0;
            try
            {
                int.TryParse(vtime, out itime_int);
            }
            catch { }
            return getTimeTH(itime_int);
        }
        public static string getTimeTH(int? vtime) {
            if (vtime==null) { return ""; }
            string itime = vtime?.ToString("D08") ?? "";
            if (itime.Substring(0, 2).Equals("00"))
            {
                itime = vtime?.ToString("D06") ?? "";
            }
            
            if (itime.Substring(0, 2).Equals("00"))
			{ 
                return "-";
            }

            var hr = itime.Substring(0, 2);
            var mi = itime.Substring(2, 2);
            var sd = itime.Substring(4, 2);

            return $"{hr}:{mi}:{sd}";
        }
		public static int getDateRow()
		{
            var dt = DateTime.Now;
            int y = dt.Year;
            y = (y < 2200) ? y + 543 : y;
            int m = dt.Month;
            int d = dt.Day;

            return int.Parse( y.ToString("D04") + m.ToString("D02") + d.ToString("D02") );
		}

        public static int getTimeRow()
        {
            var dt = DateTime.Now;
            int hh = dt.Hour;
            int mi = dt.Minute;
            int ss = dt.Second;

            return int.Parse(hh.ToString("D02") + mi.ToString("D02") + ss.ToString("D02"));
        }

        public static String GenID(long? pid)
        {
            if (pid == null) { return "-"; }
            long xpid = pid.Value;
            if (xpid == 0) { return "-"; }
            return xpid.ToString("0-0000-00000-00-0");
        }

        public static String GenAddress(string hno, string moo, string trok, string soi, string thanon, string tambol, string amphoe, string province, string rcode = "", string classname = "") {
            string addr = "";
            if (!string.IsNullOrEmpty(hno))
            {
                addr += $"<span class=\"{classname}\">{hno.Trim()}</span> ";
            }

            int mm = 0;
            int.TryParse(moo, out mm);
            
            if (!string.IsNullOrEmpty(moo) && mm > 0)
            {
                addr += $"<span class=\"{classname}\">ม.{int.Parse(moo).ToString()}</span> ";
            } else if (!string.IsNullOrEmpty(moo) && moo != "0")
            {
                addr += $"<span class=\"{classname}\">{moo}</span> ";
            }

            if (!string.IsNullOrEmpty(trok))
            {
                addr += $"<span class=\"{classname}\">ตรอก{trok}</span> ";
            }

            if (!string.IsNullOrEmpty(soi))
            {
                addr += $"<span class=\"{classname}\">ซ.{soi}</span> ";
            }

            if (!string.IsNullOrEmpty(thanon))
            {
                addr += $"<span class=\"{classname}\">ถ.{thanon}</span> ";
            }
            if (!string.IsNullOrEmpty(tambol) && !string.IsNullOrEmpty(amphoe) && !string.IsNullOrEmpty(province))
            {
                if (!string.IsNullOrEmpty(rcode) && rcode.Substring(0, 2).Equals("10"))
                {
                    addr += $"<span class=\"{classname}\">แขวง{tambol}</span> ";
                    addr += $"<span class=\"{classname}\">แขวง{amphoe}</span> ";
                    addr += $"<span class=\"{classname}\">แขวง{province}</span> ";
                }
                else
                {
                    addr += $"<span class=\"{classname}\">ต.{tambol}</span> ";
                    addr += $"<span class=\"{classname}\">อ.{amphoe}</span> ";
                    addr += $"<span class=\"{classname}\">จ.{province}</span> ";
                }
            }

            return string.IsNullOrEmpty(addr) ? "-" : addr;
        }

        public static String toJsonData(object? obj = null)
        {
            if (obj == null) { return ""; }
            return obj.ToJsonString().Replace("\"", "\\\"");
        }

        public static string CheckBoxString(string key, int value, string name)
        {
            string str = "";
            if (value == 1)
            {
                str = $"<div class=\"form-check form-switch\">\r\n                        <input type=\"checkbox\" class=\"form-check-input\" name=\"{key}\" checked />\r\n                        <label class=\"form-check-label\">{name}</label></div>";
            } else
            {
                str = $"<div class=\"form-check form-switch\">\r\n                        <input type=\"checkbox\" class=\"form-check-input\" name=\"{key}\" />\r\n                        <label class=\"form-check-label\">{name}</label></div>";
            }

            return str;
        }

        public static string CardList(List<CardBP1>? lcard) 
        {
            if (lcard == null) return "";
            if (lcard.Count == 0) return "";
            var scard = lcard.Select(c =>  $"'{c.documentNumber}'" );
            string strout = string.Join(",", scard);
            return strout;
        }

        public static async Task< List<SelectListItem> >getProvinceTH(string domains)
        {
            var list = new List<SelectListItem>();
            Rest.Api api = new Rest.Api();
            api.setDomain(domains);
            try
            {
                var res = await api.GET("/home/ws/catm");
                if (res.IsSuccessStatusCode)
                {
                    var catm = await res.getJsonObjectAsync<CItem>();
                    if (catm != null)
                    {
                        list = catm?.dataItem?.ConvertAll(a =>
                        {
                            return new SelectListItem()
                            {
                                Text = a.desc.ToString(),
                                Value = a.code.ToString(),
                                Selected = false
                            };
                        });
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return list;
        }

        public static async Task<Api.Error> SaveLog(IApi api, string desc) {
            Api.Error err = new();
            try
            {
                VLog l = new();
                l.logdate = getDateRow();
                l.logtime = getTimeRow();
                l.pid = api?.getUserLK()?.pid ?? 0L;
                l.desc = desc;
                var res = await api.POST("/api/log", l);
                if ( res.IsSuccessStatusCode )
                {
                    err = await res.getJsonObjectAsync<Error>();
                    if (err.fail)
                    {
                        Console.WriteLine(err.msg);
                    }
                } else
                {
                    err.msg = "ไม่สามารถเชื่อมต่อ API ได้";
                    err.fail = true; 
                    if (err.fail)
                    {
                        Console.WriteLine(err.msg);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                err.msg = "ไม่สามารถเชื่อมต่อ API ได้";
                err.fail = true;
            }
            return err;
        }
    }
}
