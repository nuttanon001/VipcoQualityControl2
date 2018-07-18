using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Models.QualityControls
{
    public class WelderNo:BaseModel
    {
        [Key]
        public int WelderNoId { get; set; }
        [StringLength(50)]
        public string WelderNoCode { get; set; }
        [StringLength(50)]
        public string TeamWelderNo { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ExprireDate { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(250)]
        public string NameThai { get; set; }
        [StringLength(250)]
        public string NameEnglish { get; set; }
        //Relation
        [StringLength(50)]
        public string EmpCode { get; set; }

    }
}
