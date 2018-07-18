using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class WorkGroupQualityControl:BaseModel
    {
        [Key]
        public int WorkGroupQualityControlId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(500)]
        public string SubEmail { get; set; }
        //FK
        //RequireQualityControl
        public ICollection<RequireQualityControl> RequireQualityControls { get; set; } = new List<RequireQualityControl>();
    }
}
