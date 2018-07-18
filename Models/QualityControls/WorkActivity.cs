﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class WorkActivity:BaseModel
    {
        [Key]
        public int WorkActivityId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public TypeWorkActivity TypeWorkActivity { get; set; }
        // FK
    }

    public enum TypeWorkActivity
    {
        QC_QA = 1,
        NDE
    }
}
