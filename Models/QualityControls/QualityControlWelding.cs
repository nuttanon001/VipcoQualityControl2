using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoQualityControl.Models.QualityControls
{
    public class QualityControlWelding:BaseModel
    {
        [Key]
        public int QualityControlWeldingId { get; set; }
        public DateTime WeldingDate { get; set; }
        [StringLength(100)]
        public string MarkNo { get; set; }
        [StringLength(100)]
        public string MarkNoPreview { get; set; }
        [StringLength(100)]
        public string WelderNo { get; set; }
        [StringLength(100)]
        public string ProcessWeld { get; set; }
        public int? JointNo { get; set; }
        public double? Thickness { get; set; }
        public double? TestLength { get; set; }
        public double? FailLength { get; set; }
        public double? Reject { get; set; }
        [StringLength(250)]
        public string Remark { get; set; }
        // Relation
        // QualityControlWelding
        [ForeignKey("ParentQcWelding")]
        public int? ParentQcWeldingId { get; set; }
        public virtual QualityControlWelding ParentQcWelding { get; set; }
        // ResponseBy
        public string ResponseBy { get; set; }
        // RequireQualityControl
        public int? RequireQualityControlId { get; set; }
        public virtual RequireQualityControl RequireQualityControl { get; set; }
        // ProjectCodeMaster
        public int? ProjectCodeMasterId { get; set; }
        // QualityControlReason
        public int? QualityControlReasonId { get; set; }
        public virtual QualityControlReason QualityControlReason { get; set; }
    }
}
