using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoQualityControl.ViewModels
{
    public class EmailTemplateViewModel
    {
        public string ToEmployeeName { get; set; }
        public string RequireNo { get; set; }
        public bool Status { get; set; }
        public string ResultDate { get; set; }
        public string LinkToApp { get; set; }
        public ICollection<RequireHasMasterProjectViewModel> ItemLists { get; set; }
    }
}
