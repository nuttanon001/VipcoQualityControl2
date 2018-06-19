import { Component, OnInit, ViewContainerRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from "@angular/common";
// Component
import { BaseMaster2ndEditionComponent } from '../../shared/base-master-2nd-edition-component';
import { QcWelder } from '../shared/qc-welder.model';
import { QcWelderService } from '../shared/qc-welder.service';
import { QcWelderCommuncateService } from '../shared/qc-welder-communcate.service';
import { AuthService } from '../../core/auth/auth.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { QcWelderTableComponent } from '../qc-welder-table/qc-welder-table.component';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';
import { subscribeOn } from 'rxjs/operator/subscribeOn';
import { $ } from 'protractor';
import { RequireStatusQc } from '../../require-qulitycontrols/shared/require-status-qc.enum';

@Component({
  selector: 'app-qc-welder-master',
  templateUrl: './qc-welder-master.component.html',
  styleUrls: ['./qc-welder-master.component.scss']
})
export class QcWelderMasterComponent extends BaseMaster2ndEditionComponent
<QcWelder, QcWelderService, QcWelderCommuncateService> {
  constructor(
    public service: QcWelderService,
    public serviceCommancate: QcWelderCommuncateService,
    public serviceAuth: AuthService,
    public serviceDialogs: DialogsService,
    public viewContainerRef: ViewContainerRef,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private serviceRequireQc: RequireQualityControlService,

  ) {
    super(service, serviceCommancate, serviceAuth,serviceDialogs,viewContainerRef);
  }
  //Parameter
  @ViewChild(QcWelderTableComponent)
  private tableComponent: QcWelderTableComponent;
  requireQc: RequireQc;
  qcWelders: Array<QcWelder>;
  backToSchedule: boolean = false;
  //////////////
  // Override //
  //////////////
  //on ngInitAngular
  ngOnInit(): void {
    super.ngOnInit();

    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("condition") || 0);
      if (key) {
        this.backToSchedule = true;
        this.qcWelders = new Array;
        this.serviceRequireQc.getOneKeyNumber({ RequireQualityControlId: key })
          .subscribe(dbData => {
            if (dbData) {
              this.requireQc = dbData;
            } else {
              this.onBack();
            }
          });
      }
    }, error => console.error(error));
  }
  
  // on detail view
  onDetailView(value?: { data: QcWelder, option: number }): void {
    //debug here
    // console.log("Value is",JSON.stringify(value));
    let empCode: string = this.authService.getAuth.EmpCode || "";
    if (value) {
      if (value.data) {
        // Only user who create can edit
        if (this.authService.getAuth && value.option !== 2) {
          if (this.authService.getAuth.LevelUser < 3) {
            if (this.authService.getAuth.UserName !== value.data.Creator) {
              this.dialogsService.error("Access Denied", "You don't have permission to access.", this.viewContainerRef);
              return;
            }
          }
        }

        if (value.option === 1) {
          this.onLoading = true;
          this.service.getOneKeyNumber(value.data)
            .subscribe(dbData => {
              if (dbData.RequireQualityControlId) { return; }
              this.onLoading = false;
              this.displayValue = dbData;
              this.openDialogQualityControl();
            }, error => this.displayValue = undefined);
        }
        else if (value.option === 2) // Replicate
        {
          this.onLoading = true;
          this.service.getOneKeyNumber(value.data)
            .subscribe(dbData => {
              this.onLoading = false;
              //Replicate Data
              this.displayValue = {
                JointNo: dbData.JointNo,
                MarkNo: dbData.MarkNo,
                MarkNoPreview: dbData.MarkNoPreview,
                ProcessWeld: dbData.ProcessWeld,
                ProjectCodeMasterId: dbData.ProjectCodeMasterId,
                ProjectCodeMasterString: dbData.ProjectCodeMasterString,
                QualityControlWeldingId: 0,
                Remark: dbData.Remark,
                TestLength: dbData.TestLength,
                Thickness: dbData.Thickness,
                WelderNo: dbData.WelderNo,
                WeldingDate: dbData.WeldingDate,
                ResponseBy: empCode
              };
              this.openDialogQualityControl();
            }, error => this.displayValue = undefined);
        }
        else if (value.option === 0) // Remove
        {
          this.onLoading = true;
          this.serviceDialogs.confirm("Warning Message", "Do you want delete this item?", this.viewContainerRef)
            .subscribe(result => {
              if (result) {
                this.service.deleteKeyNumber(value.data.QualityControlWeldingId)
                  .subscribe(dbData => {});
              }
              this.onLoading = false;
            });
        }
      } else {
        this.displayValue = {
          QualityControlWeldingId: 0,
          ResponseBy: empCode
        };
        this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
          .subscribe(projectData => {
            if (projectData) {
              this.displayValue.ProjectCodeMasterId = projectData.ProjectCodeMasterId;
              this.displayValue.ProjectCodeMasterString = `${projectData.ProjectCode}/${projectData.ProjectName}`;
              // Open dialog quality control
              this.openDialogQualityControl();
            }
          });
      }
    } else {
      this.displayValue = {
        QualityControlWeldingId: 0,
        ResponseBy: empCode
      };
      this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
        .subscribe(projectData => {
          if (projectData) {
            this.displayValue.ProjectCodeMasterId = projectData.ProjectCodeMasterId;
            this.displayValue.ProjectCodeMasterString = `${projectData.ProjectCode}/${projectData.ProjectName}`;
            // Open dialog quality control
            this.openDialogQualityControl();
          }
        });
    }
  }

  // on Open dialog Quality control welding
  openDialogQualityControl(): void {
    this.serviceDialogs.dialogCreateOrUpdateWelderInfo(this.viewContainerRef, this.displayValue)
      .subscribe(returnData => {
        if (returnData) {
          this.displayValue = returnData;
          //debug here
          // console.log(JSON.stringify(this.displayValue));

          this.onSubmit();
        } else {
          this.displayValue = undefined;
        }
      });
  }

  //table Reload
  onReloadData(): void {
    this.tableComponent.reloadData();
  }

  // on save complete
  onSaveComplete(): void {
    //this.dialogsService.context("System message", "Save completed.", this.viewContainerRef)
    //  .subscribe(result => {
    //  });

    this.onBack();
    this.ShowDetail = false;
    this.displayValue = undefined;
    this.onReloadData();
  }

  // on back from report
  onBack(): void {
    if (this.backToSchedule) {
      this.location.back();
    }
  }

  //onRequireQcSelected
  onRequireQcSelected(selectes: Array<QcWelder>): void {
    if (selectes) {
      this.qcWelders = new Array;
      // this.qcWelders = [...selectes];
      selectes.forEach((qcWelder, index) => {
        if (!this.qcWelders.find(value => value.QualityControlWeldingId === qcWelder.QualityControlWeldingId)) {
          this.qcWelders.push(Object.assign({}, qcWelder));
        } 
      });
    }
  }

  //onUpdateQcWelderToRequierQc
  onUpdateQcWelderToRequierQc(): void {
    if (this.qcWelders && this.requireQc) {
      this.serviceDialogs.confirm("Confirm Message", "Do you want to send information to QC-NDE?", this.viewContainerRef)
        .subscribe(result => {
          if (result) {
            this.onLoading;

            this.qcWelders.forEach((item, index) => {
              item.RequireQualityControlId = this.requireQc.RequireQualityControlId;
              item.Modifyer = this.authService.getAuth.UserName;
              this.service.updateModelWithKey(item).subscribe();
            });

            this.requireQc.RequireStatus = RequireStatusQc.WeldingReq;
            this.serviceRequireQc.updateModelWithKey(this.requireQc)
              .subscribe();

            setTimeout(() => {
              this.onLoading = false;
              this.onSaveComplete();
            }, 1500);
          }
        })
    }
  }
}
