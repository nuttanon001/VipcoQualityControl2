// Angular Core
import { Component, OnInit, Input, ViewContainerRef } from '@angular/core';
import { FormBuilder, Validators, AbstractControl } from '@angular/forms';
// rxjs
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
// Models
import { AutoComplate } from '../../shared/auto-complate';
import { ResultAutoComplate } from '../../shared/result-auto-complate';
// Services
import { MasterListService } from '../../../master-lists/shared/master-list.service';
// Extends
import { BaseInfoDialogComponent } from '../../../shared/base-info-dialog-component';
import { RequireQcWelder } from '../../../require-qc-welders/shared/require-qc-welder.model';
import { DialogsService } from '../../shared/dialogs.service';
import { WelderNo } from '../../../welder-no/shared/welder-no.model';
import { WelderStatus } from '../../../require-qc-welders/shared/welder-status.enum';
import { WelderProcess } from '../../../require-qc-welders/shared/welder-process.enum';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { WelderNoDialogComponent } from '../../welder-no-dialog/welder-no-dialog.component';

@Component({
  selector: 'app-require-qc-welder-info-dialog',
  templateUrl: './require-qc-welder-info-dialog.component.html',
  styleUrls: ['./require-qc-welder-info-dialog.component.scss']
})
export class RequireQcWelderInfoDialogComponent extends BaseInfoDialogComponent<RequireQcWelder> {
  constructor(
    private fb: FormBuilder,
    private service: MasterListService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private dialog: MatDialog
  ) { super() }
  // parameter
  @Input() NeedWelder: boolean = false;
  welderStatus: Array<{ data: WelderStatus, lable: string }>;
  welderProcess: Array<{ data: WelderProcess, lable: string }>;
  resultAutoComplate: Array<ResultAutoComplate>;
  // methods
  buildForm(): void {
    if (!this.welderStatus) {
      this.welderStatus = new Array;
      this.welderStatus.push({ data: WelderStatus.Accepted, lable: WelderStatus[WelderStatus.Accepted] });
      this.welderStatus.push({ data: WelderStatus.AcceptedAfterRepaired, lable: WelderStatus[WelderStatus.AcceptedAfterRepaired] });
      this.welderStatus.push({ data: WelderStatus.Rejected, lable: WelderStatus[WelderStatus.Rejected] });
    }
    if (!this.welderProcess) {
      this.welderProcess = new Array;
      this.welderProcess.push({ data: WelderProcess.FCAW, lable: WelderProcess[WelderProcess.FCAW] })
      this.welderProcess.push({ data: WelderProcess.FCAW_SAW, lable: WelderProcess[WelderProcess.FCAW_SAW] })
      this.welderProcess.push({ data: WelderProcess.GTAW, lable: WelderProcess[WelderProcess.GTAW] })
      this.welderProcess.push({ data: WelderProcess.GTAW_FCAW, lable: WelderProcess[WelderProcess.GTAW_FCAW] })
      this.welderProcess.push({ data: WelderProcess.GTAW_SMAW, lable: WelderProcess[WelderProcess.GTAW_SMAW] })
      this.welderProcess.push({ data: WelderProcess.SAW, lable: WelderProcess[WelderProcess.SAW] })
      this.welderProcess.push({ data: WelderProcess.SMAW, lable: WelderProcess[WelderProcess.SMAW] })
      this.welderProcess.push({ data: WelderProcess.SW, lable: WelderProcess[WelderProcess.SW] })
    }

    this.InfoValueForm = this.fb.group({
      RequireHasWelderId: [this.InfoValue.RequireHasWelderId],
      VTStaus: [this.InfoValue.VTStaus,],
      QcStatus: [this.InfoValue.QcStatus],
      WelderProcess: [this.InfoValue.WelderProcess,
      [
        Validators.required
      ]
      ],
      Wps: [this.InfoValue.Wps,[Validators.maxLength(100)]],
      WelderDate: [this.InfoValue.WelderDate,
      [
        Validators.required,
      ]
      ],
      PercentNDE: [this.InfoValue.PercentNDE,
      [
        Validators.min(0),
      ]
      ],
      Remark: [this.InfoValue.Remark,
      [
        Validators.maxLength(200),
      ]
      ],
      RequireHasMasterProjectId: [this.InfoValue.RequireHasMasterProjectId],
      RequireHasMasterProject: this.fb.group({
        MasterProjectListId: [this.InfoValue.RequireHasMasterProject.MasterProjectListId],
        Name: [this.InfoValue.RequireHasMasterProject.Name,
        [
          Validators.maxLength(50)
        ]
        ],
        Description: [this.InfoValue.RequireHasMasterProject.Description,
        [
          Validators.maxLength(200)
        ]
        ],
        Remark: [this.InfoValue.RequireHasMasterProject.Remark,
        [
          Validators.maxLength(200)
        ]
        ],
        DrawingNo: [this.InfoValue.RequireHasMasterProject.DrawingNo,
        [
          Validators.required,
          Validators.maxLength(200)
        ]
        ],
        MarkNo: [this.InfoValue.RequireHasMasterProject.MarkNo,
        [
          Validators.required,
          Validators.maxLength(200),
        ]
        ],
        Length: [this.InfoValue.RequireHasMasterProject.Length],
        Width: [this.InfoValue.RequireHasMasterProject.Width],
        Heigth: [this.InfoValue.RequireHasMasterProject.Heigth],
        Weigth: [this.InfoValue.RequireHasMasterProject.Weigth],
        Quantity: [this.InfoValue.RequireHasMasterProject.Quantity,
        [
          Validators.min(1),
          Validators.required,
        ]
        ],
        UnitNo: [this.InfoValue.RequireHasMasterProject.UnitNo,
        [
          Validators.min(0)
        ]
        ],
        Box: [this.InfoValue.RequireHasMasterProject.Box,
        [
          Validators.min(0)
        ]
        ],
        Revised: [this.InfoValue.RequireHasMasterProject.Revised],
        ProjectCodeDetailId: [this.InfoValue.RequireHasMasterProject.ProjectCodeDetailId],
        ProjectCodeDetailString: [this.InfoValue.RequireHasMasterProject.ProjectCodeDetailString],
        // Update Welder
        Thickness: [this.InfoValue.RequireHasMasterProject.Thickness, [Validators.min(0)]],
        Thickness2: [this.InfoValue.RequireHasMasterProject.Thickness2, [Validators.min(0)]],
        JointNumber: [this.InfoValue.RequireHasMasterProject.JointNumber, [Validators.min(0)]],
        GradeMaterial1: [this.InfoValue.RequireHasMasterProject.GradeMaterial1, [Validators.maxLength(200)]],
        GradeMaterial2: [this.InfoValue.RequireHasMasterProject.GradeMaterial2, [Validators.maxLength(200)]],
        TypeMaterial1: [this.InfoValue.RequireHasMasterProject.TypeMaterial1, [Validators.maxLength(200)]],
        TypeMaterial2: [this.InfoValue.RequireHasMasterProject.TypeMaterial2, [Validators.maxLength(200)]],
        // BaseModel
        Creator: [this.InfoValue.RequireHasMasterProject.Creator],
        CreateDate: [this.InfoValue.RequireHasMasterProject.CreateDate],
        Modifyer: [this.InfoValue.RequireHasMasterProject.Modifyer],
        ModifyDate: [this.InfoValue.RequireHasMasterProject.ModifyDate],
      }),
      WelderNo1Id: [this.InfoValue.WelderNo1Id],
      WelderNo1Name: [this.InfoValue.WelderNo1Name],
      WelderNo2Id: [this.InfoValue.WelderNo2Id],
      WelderNo2Name: [this.InfoValue.WelderNo2Name],
      WelderNo1String: [this.InfoValue.WelderNo1String],
      WelderNo2String: [this.InfoValue.WelderNo2String],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
      //ViewModel
      VTStausString: [this.InfoValue.VTStausString],
      WelderProcessString: [this.InfoValue.WelderProcessString]
    });

    this.InfoValueForm.valueChanges.pipe(debounceTime(250), distinctUntilChanged()).subscribe(data => {
      if (!this.InfoValueForm) { return; }
      if (this.InfoValueForm.valid) {
        this.InfoValue = this.InfoValueForm.value;
        // String
        this.InfoValue.VTStausString = WelderStatus[this.InfoValue.VTStaus];
        this.InfoValue.WelderProcessString = WelderProcess[this.InfoValue.WelderProcess];
        this.SubmitOrCancel.emit({ data: this.InfoValue, force: false });
      }
    });

    // Get Control
    this.InfoValueForm.get("RequireHasMasterProject.MarkNo").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "MarkNo", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("RequireHasMasterProject.DrawingNo").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "DrawingNo", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("RequireHasMasterProject.GradeMaterial1").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "GradeMaterial1", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("RequireHasMasterProject.GradeMaterial2").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "GradeMaterial2", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("RequireHasMasterProject.TypeMaterial1").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "TypeMaterial1", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("RequireHasMasterProject.TypeMaterial2").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "TypeMaterial2", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
  }
  // fill auto complate
  fillAutoComplated(fillAuto: AutoComplate) {
    this.service.getAutoComplateEdition(fillAuto)
      .subscribe(resultAuto => {
        this.resultAutoComplate = new Array;
        if (resultAuto) {
          this.resultAutoComplate = [...resultAuto];
        }
      });
  }

  openDialogs(type: string) {
    if (type) {
      let config: MatDialogConfig = new MatDialogConfig();
      config.viewContainerRef = this.viewContainerRef;
      config.data = 0;
      config.hasBackdrop = true;

      if (type.indexOf("WelderNo1Id") !== -1) {
        const dialogRef = this.dialog.open(WelderNoDialogComponent, config);
        dialogRef.afterClosed().subscribe(result => {
          this.InfoValueForm.patchValue({
            WelderNo1Id: result ? result.WelderNoId : undefined,
            WelderNo1Name: result ? result.WelderNoCode : undefined,
            WelderNo1String: result ? result.WelderNoCode : undefined,
          });
        });
      } else if (type.indexOf("WelderNo2Id") !== -1) {
        const dialogRef = this.dialog.open(WelderNoDialogComponent, config);
        dialogRef.afterClosed().subscribe(result => {
          this.InfoValueForm.patchValue({
            WelderNo2Id: result ? result.WelderNoId : undefined,
            WelderNo2Name: result ? result.WelderNoCode : undefined,
            WelderNo2String: result ? result.WelderNoCode : undefined,
          });
        });
      }
    }
  }
}
