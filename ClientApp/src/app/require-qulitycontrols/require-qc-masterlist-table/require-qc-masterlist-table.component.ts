// Angular Core
import { Component, Input } from "@angular/core";
// Components
import { BaseTableFontData } from "../../shared/base-table-fontdata.component";
// Module
import { MasterList } from "../../master-lists/shared/master-list.model";

@Component({
  selector: 'app-require-qc-masterlist-table',
  templateUrl: './require-qc-masterlist-table.component.html',
  styleUrls: ['./require-qc-masterlist-table.component.scss']
})
export class RequireQcMasterlistTableComponent extends BaseTableFontData<MasterList> {
  /** custom-mat-table ctor */
  constructor() {
    super();
    this.displayedColumns = ["edit" , "DrawingNo", "MarkNo", "Name", "UnitNo", "Quantity",
      "JointNumber", "Thickness", "TypeMaterial1", "GradeMaterial1",
      "VTStaus",
      "WelderProcess",
      "WelderDate",
      "PercentNDE",
      "WelderNo",
      "FailQuantity",
      "RemarkExter"];
  }

  @Input() isDialog: number = 0;
  @Input() colMode: string = "normal";

  //////////////
  // Override //
  //////////////
  ngOnInit(): void {
    if (this.colMode.indexOf("normal") !== -1) {
      this.displayedColumns = [
        "edit", "DrawingNo", "MarkNo", "Name", "UnitNo", "Quantity",
        "JointNumber", "Thickness", "TypeMaterial1", "GradeMaterial1"];
    } else if (this.colMode.indexOf("welder") !== -1) {
      this.displayedColumns = [
        "edit", "DrawingNo", "MarkNo", "Name", "UnitNo", "Quantity",
        "VTStaus", "WelderProcess", "WelderDate", "PercentNDE", "WelderNo",
        "JointNumber", "Thickness", "TypeMaterial1", "GradeMaterial1"];
    } else if (this.colMode.indexOf("forFail") !== -1) {
      this.displayedColumns = [
        "edit", "DrawingNo", "MarkNo", "Name", "UnitNo", "Quantity",
        "JointNumber", "Thickness", "TypeMaterial1", "GradeMaterial1",
        "FailQuantity", "RemarkExter"];
    }

    super.ngOnInit();
  }
}
