using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class WelderHasProjectViewModel:BaseModel
    {
        public int WelderHasProjectId { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public int? WelderNoId { get; set; }
        public int? ProjectCodeMasterId { get; set; }
        public string ProjectCodeMasterString { get; set; }
        public string EmployeeString { get; set; }
    }
}
