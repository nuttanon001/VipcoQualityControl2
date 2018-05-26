using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Models.QualityControls
{
    public class QualityControlReason:BaseModel
    {
        [Key]
        public int QualityControlReasonId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Remark { get; set; }
        //FK
        //RequireHasMasterProject
        public virtual ICollection<RequireHasMasterProject> RequireHasMasterProjects { get; set; }
    }
}
