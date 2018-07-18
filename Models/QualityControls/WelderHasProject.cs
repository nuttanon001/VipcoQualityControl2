using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Models.QualityControls
{
    public class WelderHasProject : BaseModel
    {
        [Key]
        public int WelderHasProjectId { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        // Relation
        // WeledNo
        public int? WelderNoId { get; set; }
        public WelderNo WelderNo { get; set; }
        //ProjectCodeMaster
        public int? ProjectCodeMasterId { get; set; }
    }
}
