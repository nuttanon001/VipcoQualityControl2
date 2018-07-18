using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoQualityControl.Models.QualityControls
{
    public class MasterProjectList:BaseModel
    {
        [Key]
        public int MasterProjectListId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(200)]
        public string DrawingNo { get; set; }
        [StringLength(200)]
        public string MarkNo { get; set; }
        public int? UnitNo { get; set; }
        public int? Box { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Heigth { get; set; }
        public double? Weigth { get; set; }
        public double? Quantity { get; set; }
        public int? Revised { get; set; }
        // Update 11/07/18 add welder information
        public double? Thickness { get; set; }
        public double? JointNumber { get; set; }
        [StringLength(200)]
        public string TypeMaterial1 { get; set; }
        [StringLength(200)]
        public string GradeMaterial1 { get; set; }
        [StringLength(200)]
        public string TypeMaterial2 { get; set; }
        [StringLength(200)]
        public string GradeMaterial2 { get; set; }
        //FK
        //ProjectCodeDetail
        public int? ProjectCodeDetailId { get; set; }

    }
}
