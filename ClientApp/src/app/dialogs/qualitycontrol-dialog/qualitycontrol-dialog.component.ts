import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
// services
import { AuthService } from "../../core/auth/auth.service";
import { RequireQualityControlService } from "../../require-qulitycontrols/shared/require-qc.service";
import { RequireHasMasterService } from "../../require-qulitycontrols/shared/require-has-master.service";
import { QualityControlService } from "../../quality-controls/shared/quality-control.service";
import { QualityControl } from "../../quality-controls/shared/quality-control.model";
import { QcReasonsService } from "../../quality-control-reasons/shared/qc-reasons.service";

@Component({
  selector: 'app-qualitycontrol-dialog',
  templateUrl: './qualitycontrol-dialog.component.html',
  styleUrls: ['./qualitycontrol-dialog.component.scss'],
  providers: [
    QualityControlService,
    RequireHasMasterService,
    RequireQualityControlService,
    QcReasonsService,
  ]
})

export class QualitycontrolDialogComponent implements OnInit {
  /** require-painting-view-dialog ctor */
  constructor(
    private service: QualityControlService,
    private serviceAuth: AuthService,
    @Inject(MAT_DIALOG_DATA) public QualityControlId: number,
    private dialogRef: MatDialogRef<number>) { }

  // Parameter
  qualityControl: QualityControl;
  canClose: boolean;

  /** Called by Angular after quality-control-dialog component initialized */
  ngOnInit(): void {
    this.canClose = false;
    if (this.QualityControlId) {
      this.service.getOneKeyNumber({ QualityControlResultId: this.QualityControlId })
        .subscribe(dbData => {
          this.qualityControl = dbData;
        }, error => this.onCancelClick());
    } else {
      this.onCancelClick();
    }
  }

  // No Click
  onCancelClick(): void {
    this.dialogRef.close();
  }
}
