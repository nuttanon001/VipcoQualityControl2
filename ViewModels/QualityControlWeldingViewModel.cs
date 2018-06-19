using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.ViewModels
{
    public class QualityControlWeldingViewModel: QualityControlWelding
    {
        public string ProjectCodeMasterString { get; set; }
        public string RequireQualityControlNo { get; set; }
    }
}
