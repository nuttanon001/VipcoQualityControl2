using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class WelderNoViewModel:BaseModel
    {
        public int WelderNoId { get; set; }
        public string WelderNoCode { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ExprireDate { get; set; }
        public string TeamWelderNo { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        //Relation
        public string EmpCode { get; set; }
        public string EmployeeString { get; set; }
    }
}
