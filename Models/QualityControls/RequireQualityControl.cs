using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoQualityControl.Models.QualityControls
{
    public class RequireQualityControl : BaseModel
    {
        [Key]
        public int RequireQualityControlId { get; set; }
        [StringLength(50)]
        public string RequireQualityNo { get; set; }
        [Required]
        public DateTime RequireDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? WelderDate { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(200)]
        public string MailReply { get; set; }
        public RequireStatus RequireStatus { get; set; }
        //FK
        // RequireQualityControl
        [ForeignKey("ParentRequireQc")]
        public int? ParentRequireQcId { get; set; }
        public RequireQualityControl ParentRequireQc { get; set; }
        // GroupMis
        public string GroupMIS { get; set; }
        // Employee
        public string RequireEmp { get; set; }
        // ProjectCodeDetail
        public int? ProjectCodeDetailId { get; set; }
        //WorkGroupQualityControl
        public int? WorkGroupQualityControlId { get; set; }
        public WorkGroupQualityControl WorkGroupQualityControl { get; set; }
        //InspectionPoint
        public int? InspectionPointId { get; set; }
        public InspectionPoint InspectionPoint { get; set; }
        //WorkActivity
        public int? WorkActivityId { get; set; }
        public WorkActivity WorkActivity { get; set; }
        //Branch
        public int? BranchId { get; set; }
        public Branch Branch { get; set; }
        // RequireHasMasterProject
        public ICollection<RequireHasMasterProject> RequireHasMasterProjects { get; set; } = new List<RequireHasMasterProject>();
        // RequireQcMoreWorkActivity
        public ICollection<RequireQcMoreWorkActvity> RequireQcMoreWorkActvities { get; set; } = new List<RequireQcMoreWorkActvity>();
        // RequireHasAttach
        public ICollection<RequireHasAttach> RequireHasAttaches { get; set; } = new List<RequireHasAttach>();
        // WorkQualityControl
        public WorkQualityControl WorkQualityControl { get; set; }
        // QualityControlResult
        public QualityControlResult QualityControlResult { get; set; }
    }

    public enum RequireStatus
    {
        Waiting = 1,
        QcResponse,
        QcChangeResponse,
        InProcess,
        Complate,
        QcFail,
        Cancel,
        Revise,
        Welding,
        WeldingReq,
        WeldingFail,
    }
}
