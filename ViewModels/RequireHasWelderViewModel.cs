using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class RequireHasWelderViewModel: RequireHasWelder
    {
        public string WelderNo1String { get; set; }
        public string WelderNo2String { get; set; }
    }
}
