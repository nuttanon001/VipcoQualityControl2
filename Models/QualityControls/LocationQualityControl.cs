﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class LocationQualityControl:BaseModel
    {
        [Key]
        public int LocationQualityControlId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        //FK
        // WorkGroupHasWorkShop
        public virtual ICollection<WorkGroupHasWorkShop> WorkGroupHasWorkShops { get; set; }
    }
}
