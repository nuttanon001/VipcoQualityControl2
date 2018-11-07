﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoQualityControl.ViewModels
{
    public class OptionRequireQualityControl
    {
        public string Filter { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        /// <summary>
        /// </summary>
        public int? Status { get; set; }
        public int? GroupQcId { get; set; }
    }
}
