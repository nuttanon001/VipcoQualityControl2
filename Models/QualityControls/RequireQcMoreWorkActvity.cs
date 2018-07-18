﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace VipcoQualityControl.Models.QualityControls
{
    public class RequireQcMoreWorkActvity:BaseModel
    {
        [Key]
        public int RequireQcMoreWorkActvityId { get; set; }
        //Relation
        //RequireQualityControl
        public int? RequireQualityControlId { get; set; }
        public RequireQualityControl RequireQualityControl { get; set; }
        //WorkActivity
        public int? WorkActivityId { get; set; }
        public WorkActivity WorkActivity { get; set; }
    }
}
