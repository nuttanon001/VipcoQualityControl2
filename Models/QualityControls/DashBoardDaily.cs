using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class DashBoardDaily : BaseModel
    {
        [Key]
        public int DashBoardDailyId { get; set; }
        public DateTime? DashBoardDate { get; set; }
        public int? TotalRequire { get; set; }
        public int? TotalRequirePass { get; set; }
        public int? TotalRequireFail { get; set; }
        public string Top1Name { get; set; }
        public int? Top1Require { get; set; }
        public string Top2Name { get; set; }
        public int? Top2Require { get; set; }
        public string Top3Name { get; set; }
        public int? Top3Require { get; set; }
    }
}
