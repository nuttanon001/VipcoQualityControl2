using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class RequireHasMasterProjectViewModel:RequireHasMasterProject
    {
        public string MarkNoString { get; set; }
        public string DrawingNo { get; set; }
        public int? UnitNo { get; set; }
        public int? Box { get; set; }
        public string Name { get; set; }
        public string QualityControlReasonString { get; set; }
    }
}
