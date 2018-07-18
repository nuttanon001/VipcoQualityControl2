using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoQualityControl.Models.QualityControls
{
    public class RequireHasWelder : BaseModel
    {
        [Key]
        public int RequireHasWelderId { get; set; }
        public WelderStatus? VTStaus { get; set; }
        public WelderStatus? QcStatus { get; set; } 
        public WelderProcess? WelderProcess { get; set; }
        public DateTime? WelderDate { get; set; }
        public double PercentNDE { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        //Relation
        //RequireHasMasterProjectId
        public int RequireHasMasterProjectId { get; set; }
        public RequireHasMasterProject RequireHasMasterProject { get; set; }
        //Welder1
        public int WelderNo1Id { get; set; }
        [StringLength(200)]
        public string WelderNo1Name { get; set; }
        //Welder2
        public int WelderNo2Id { get; set; }
        [StringLength(200)]
        public string WelderNo2Name { get; set; }

    }
    public enum WelderStatus
    {
        Accepted = 1,
        AcceptedAfterRepaired,
        Rejected
    }
    public enum WelderProcess
    {
        FCAW = 1,
        SMAW,
        GTAW,
        SAW,
        SW,
        GTAW_SMAW,
        FCAW_SAW,
        GTAW_FCAW
    }
}
