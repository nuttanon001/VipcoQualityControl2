using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace VipcoQualityControl.Models.QualityControls
{
    public class WorkGroupHasWorkShop:BaseModel
    {
        [Key]
        public int WorkGroupHasWorkShopId { get; set; }
        // FK
        //GroupMis
        public string GroupMis { get; set; }
        // LocationQualityControl
        public int? LocationQualityControlId { get; set; }
        public virtual LocationQualityControl LocationQualityControl { get; set; }
    }
}
