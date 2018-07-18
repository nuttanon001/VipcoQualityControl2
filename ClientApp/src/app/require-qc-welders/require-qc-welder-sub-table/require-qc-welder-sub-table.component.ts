import { Component, OnInit } from '@angular/core';
import { BaseTableFontData } from '../../shared/mk2/base-table-fontdata.component';
import { RequireQcWelder } from '../shared/require-qc-welder.model';
import { WelderStatus } from '../shared/welder-status.enum';
import { WelderProcess } from '../shared/welder-process.enum';

@Component({
  selector: 'app-require-qc-welder-sub-table',
  templateUrl: './require-qc-welder-sub-table.component.html',
  styleUrls: ['./require-qc-welder-sub-table.component.scss']
})
export class RequireQcWelderSubTableComponent extends BaseTableFontData<RequireQcWelder> {
  constructor() {
    super();
    this.displayedColumns = ["Command", "VTStaus", "WelderProcess", "WelderDate", "PercentNDE","WelderNo", "DrawingNo", "MarkNo", "Name", "UnitNo", "Quantity",
      "JointNumber", "Thickness", "TypeMaterial1", "GradeMaterial1", "FailQuantity", "RemarkExter"];
  }
}
