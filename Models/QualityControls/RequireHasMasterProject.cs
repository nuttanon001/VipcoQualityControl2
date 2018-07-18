using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class RequireHasMasterProject:BaseModel
    {
        [Key]
        public int RequireHasMasterProjectId { get; set; }
        public double? Quantity { get; set; }
        public double? PassQuantity { get; set; }
        //FK
        // RequireHasWelder
        public RequireHasWelder RequireHasWelder { get; set; }
        // MasterProjectList
        public int? MasterProjectListId { get; set; }
        public MasterProjectList MasterProjectList { get; set; }
        // RequireQualityControl
        public int? RequireQualityControlId { get; set; }
        public RequireQualityControl RequireQualityControl { get; set; }
        // QualityControlReason
        public int? QualityControlReasonId { get; set; }
        public QualityControlReason QualityControlReason { get; set; }
    }
}
