using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace CISApps.Models.Linkage.Department
{
    public class Child
    {
        public virtual List<Pop> child { set; get; } = new List<Pop>();
        public virtual int totalChild { get; set; }
    }
}
