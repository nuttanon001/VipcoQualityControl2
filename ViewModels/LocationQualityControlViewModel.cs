﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class LocationQualityControlViewModel:LocationQualityControl
    {
        public int? TotalGroup { get; set; }
        public string GroupMisString { get; set; }
    }
}
