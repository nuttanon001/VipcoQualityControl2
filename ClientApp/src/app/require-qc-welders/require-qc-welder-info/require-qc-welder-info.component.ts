import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { BaseInfoComponent } from '../../shared/mk2/base-info-component';
import { RequireQcWelder } from '../shared/require-qc-welder.model';
import { RequireQcWelderService } from '../shared/require-qc-welder.service';
import { RequireQcWelderCommunicateService } from '../shared/require-qc-welder-communicate.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { FormBuilder, Validators } from '@angular/forms';
import { WelderStatus } from '../shared/welder-status.enum';
import { WelderProcess } from '../shared/welder-process.enum';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';
import { WorkGroupQc } from '../../workgroup-qulitycontrols/shared/workgroup-qc.model';
import { AttachFile } from '../../shared/attach-file.model';
import { Branch } from '../../branchs/shared/branch.model';
import { InspectionPoint } from '../../inspection-points/shared/inspection-point.model';
import { WorkActivity } from '../../work-activities/shared/work-activity.model';
import { RequireMoreWorkactivityService } from '../../require-qulitycontrols/shared/require-more-workactivity.service';
import { BranchService } from '../../branchs/shared/branch.service';
import { MasterListService } from '../../master-lists/shared/master-list.service';
import { EmployeeGroupMisService } from '../../employees/shared/employee-group-mis.service';
import { WorkActivityService } from '../../work-activities/shared/work-activity.service';
import { WorkGroupQcService } from '../../workgroup-qulitycontrols/shared/workgroup-qc.service';
import { InspectionPointService } from '../../inspection-points/shared/inspection-point.service';
import { AuthService } from '../../core/auth/auth.service';
import { TypeworkActivity } from '../../work-activities/shared/typework-activity.enum';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';
import { RequireQcHasMasterList } from '../../require-qulitycontrols/shared/require-qc-has-master-list.model';

@Component({
  selector: 'app-require-qc-welder-info',
  templateUrl: './require-qc-welder-info.component.html',
  styleUrls: ['./require-qc-welder-info.component.scss']
})
export class RequireQcWelderInfoComponent extends BaseInfoComponent<RequireQc, RequireQcWelderService, RequireQcWelderCommunicateService> {
  constructor(
    service: RequireQcWelderService,
    serviceCom: RequireQcWelderCommunicateService,
    private serviceRequreQc: RequireQualityControlService,
    private serviceRequireMoreActivity: RequireMoreWorkactivityService,
    private serviceBranch: BranchService,
    private serviceGroupMis: EmployeeGroupMisService,
    private serviceWorkActivity: WorkActivityService,
    private serviceWorkGroupQc: WorkGroupQcService,
    private serviceInspection: InspectionPointService,
    private serviceAuth: AuthService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder
  ) {
    super(service, serviceCom);
  }
  ///////////////
  // Parameter //
  ///////////////
  indexItem: number;
  branchs: Array<Branch>;
  inspections: Array<InspectionPoint>;
  workActivities: Array<WorkActivity>;
  workGroupQcs: Array<WorkGroupQc>;
  attachFiles: Array<AttachFile>;
  forFail: boolean = false;
  //On get data from api
  onGetDataByKey(InfoValue?: RequireQc): void {
    if (InfoValue) {
      console.log(JSON.stringify(InfoValue));

      if (InfoValue.RequireQualityControlId) {
        this.serviceRequreQc.getOneKeyNumber(InfoValue)
          .subscribe(dbData => {
            this.InfoValue = Object.assign({}, dbData);;
            this.InfoValue.RequireQcTime = dbData.RequireQcTimeString;

            this.InfoValue.WelderDate = new Date;
            // this.editValue.RequireQcTimeString = (new Date).toLocaleTimeString("th-TH", { hour12: false });
            this.InfoValue.WelderTimeString = (new Date(this.InfoValue.WelderDate.getTime() + 60 * 60000)).toLocaleTimeString("th-TH", { hour12: false });
            this.InfoValue.WelderTime = this.InfoValue.WelderTimeString;

            if (this.InfoValue.RequireQualityControlId) {
              this.service.getByMasterId(dbData.RequireQualityControlId)
                .subscribe((RequireHasMaster: Array<RequireQcHasMasterList>) => {
                  this.intiArray(RequireHasMaster);
                });
            }
          }, error => console.error(error), () => this.buildForm());
      }
      else {
        this.InfoValue = Object.assign({}, InfoValue);
        this.InfoValue.RequireQcTime = InfoValue.RequireQcTimeString;

        this.InfoValue.WelderDate = new Date;
        this.InfoValue.WelderTimeString = (new Date(this.InfoValue.WelderDate.getTime() + 60 * 60000)).toLocaleTimeString("th-TH", { hour12: false });
        this.InfoValue.WelderTime = this.InfoValue.WelderTimeString;
        this.buildForm();
        this.intiArray(InfoValue.RequireHasMasterProjects);
      }
    }
  }
  //Inti Array
  intiArray(RequireHasMaster?: Array<RequireQcHasMasterList>): void {
    this.InfoValue.RequireQcWelder = new Array;
    if (RequireHasMaster) {
      RequireHasMaster.forEach((item, index) => {
        // if has welder
        if (item.RequireHasWelder) {
          this.InfoValue.RequireQcWelder.push({
            RequireHasWelderId: item.RequireHasWelder.RequireHasWelderId,
            VTStaus: item.RequireHasWelder.VTStaus,
            QcStatus: item.RequireHasWelder.QcStatus,
            WelderProcess: item.RequireHasWelder.WelderProcess,
            WelderDate: item.RequireHasWelder.WelderDate,
            PercentNDE: item.RequireHasWelder.PercentNDE,
            Remark: item.RequireHasWelder.Remark,
            //Relation
            //RequireHasMasterProjectId
            RequireHasMasterProjectId: item.RequireHasWelder.RequireHasMasterProjectId,
            RequireHasMasterProject: item.MasterProjectList,
            WelderNo1Id: item.RequireHasWelder.WelderNo1Id,
            WelderNo1Name: item.RequireHasWelder.WelderNo1Name,
            WelderNo2Id: item.RequireHasWelder.WelderNo2Id,
            WelderNo2Name: item.RequireHasWelder.WelderNo2Name,
            //ViewModel
            WelderNo1String: item.RequireHasWelder.WelderNo1String,
            WelderNo2String: item.RequireHasWelder.WelderNo2String,
            VTStausString: WelderStatus[item.RequireHasWelder.VTStaus],
            WelderProcessString: WelderProcess[item.RequireHasWelder.WelderProcess],
          });
        } else {
          this.InfoValue.RequireQcWelder.push({
            RequireHasWelderId: 0,
            WelderDate: new Date,
            //Relation
            //RequireHasMasterProjectId
            RequireHasMasterProjectId: item.RequireHasMasterProjectId,
            RequireHasMasterProject: item.MasterProjectList,
          });
        }
      });
      //Patch value to Form
      this.InfoValueForm.patchValue({
        RequireQcWelder: this.InfoValue.RequireQcWelder
      });
    } else {
      this.InfoValue.RequireQcWelder = new Array;
    }
  }

  // build form
  buildForm(): void {
    // GetData
    this.getBranchs();
    this.getAttach();
    this.getInspectionPoint();
    this.getWorkActivity();
    this.getWorkGroupQualityControl();
    // Form
    this.InfoValueForm = this.fb.group({
      RequireQualityControlId: [this.InfoValue.RequireQualityControlId],
      RequireQualityNo: [this.InfoValue.RequireQualityNo],
      RequireDate: [this.InfoValue.RequireDate,
      [
        Validators.required,
      ]
      ],
      ResponseDate: [this.InfoValue.ResponseDate],
      WelderDate: [this.InfoValue.WelderDate],
      Description: [this.InfoValue.Description,
      [
        Validators.maxLength(200)
      ]
      ],
      Remark: [this.InfoValue.Remark,
      [
        Validators.maxLength(200)
      ]
      ],
      MailReply: [this.InfoValue.MailReply],
      RequireStatus: [this.InfoValue.RequireStatus],
      //FK
      ParentRequireQcId: [this.InfoValue.ParentRequireQcId],
      GroupMIS: [this.InfoValue.GroupMIS],
      RequireEmp: [this.InfoValue.RequireEmp],
      ProjectCodeDetailId: [this.InfoValue.ProjectCodeDetailId],
      WorkGroupQualityControlId: [this.InfoValue.WorkGroupQualityControlId,
      [
        Validators.required
      ]
      ],
      InspectionPointId: [this.InfoValue.InspectionPointId,
      [
        Validators.required
      ]
      ],
      WorkActivityId: [this.InfoValue.WorkActivityId],
      BranchId: [this.InfoValue.BranchId,
      [
        Validators.required
      ]
      ],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
      // ViewModel
      GroupMISString: [this.InfoValue.GroupMISString,
      [
        Validators.required
      ]
      ],
      RequireEmpString: [this.InfoValue.RequireEmpString,
      [
        Validators.required
      ]
      ],
      ProjectCodeDetailString: [this.InfoValue.ProjectCodeDetailString,
      [
        Validators.required
      ]
      ],
      WorkGroupQualityControlString: [this.InfoValue.WorkGroupQualityControlString],
      InspectionPointString: [this.InfoValue.InspectionPointString],
      WorkActivityString: [this.InfoValue.WorkActivityString],
      BranchString: [this.InfoValue.BranchString],
      RequireStatusString: [this.InfoValue.RequireStatusString],
      MasterLists: [this.InfoValue.MasterLists],
      RequireQcWelder: [this.InfoValue.RequireQcWelder],
      RequireQcTime: [this.InfoValue.RequireQcTime],
      WelderTime: [this.InfoValue.WelderTime],
      MoreWorkActvities: [this.InfoValue.MoreWorkActvities],
      // Attach File
      AttachFile: [this.InfoValue.AttachFile],
      RemoveAttach: [this.InfoValue.RemoveAttach],
    });
    this.InfoValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
  }

  // get branchs
  getBranchs(): void {
    if (!this.branchs) {
      this.branchs = new Array;
    }
    if (this.branchs) {
      this.serviceBranch.getAll()
        .subscribe(dbBranch => {
          this.branchs = [...dbBranch.sort((item1, item2) => {
            if (item1.Name > item2.Name) {
              return 1;
            }
            if (item1.Name < item2.Name) {
              return -1;
            }
            return 0;
          })];

          if (!this.InfoValue.BranchId) {
            this.InfoValue.BranchId = this.branchs.find(item => item.Name.toLowerCase() === "vipco2").BranchId || undefined;
            this.InfoValueForm.patchValue({
              BranchId: this.InfoValue.BranchId
            });
          }
        })
    }
  }

  // get inspection-point
  getInspectionPoint(): void {
    if (!this.inspections) {
      this.inspections = new Array;
    }

    if (this.inspections) {
      this.serviceInspection.getAll()
        .subscribe(dbInspection => {
          this.inspections = [...dbInspection];
        });
    }
  }

  // get work activity
  getWorkActivity(): void {
    if (!this.workActivities) {
      this.workActivities = new Array;
    }

    if (this.workActivities) {
      this.serviceWorkActivity.getAll()
        .subscribe(dbWorkActivity => {
          this.workActivities = [...dbWorkActivity.filter((item) => item.TypeWorkActivity === TypeworkActivity.NDE)];
          let id: number = this.InfoValue.RequireQualityControlId || this.InfoValue.ParentRequireQcId;
          if (id) {
            this.serviceRequireMoreActivity.getByMasterId(id)
              .subscribe(dbData => {
                if (!this.InfoValue.MoreWorkActvities) {
                  this.InfoValue.MoreWorkActvities = new Array;
                }

                if (dbData) {
                  dbData.forEach((item) => {
                    let copy = this.workActivities.find((work) => work.WorkActivityId == item.WorkActivityId);
                    // console.log(JSON.stringify(copy));
                    if (copy) {
                      this.InfoValue.MoreWorkActvities.push(copy);
                    }
                  });
                  this.InfoValueForm.patchValue({
                    MoreWorkActvities: this.InfoValue.MoreWorkActvities
                  });
                }
              });
          }
        });
    }
  }

  // get work group quality control
  getWorkGroupQualityControl(): void {
    if (!this.workGroupQcs) {
      this.workGroupQcs = new Array;
    }

    if (this.workGroupQcs) {
      this.serviceWorkGroupQc.getAll()
        .subscribe(dbWorkGroupQc => {
          this.workGroupQcs = [...dbWorkGroupQc];
        });
    }
  }

  // get employee group mis
  getEmployeeGroupMisByEmpCode(EmpCode: string): void {
    if (EmpCode) {
      this.serviceGroupMis.getGroupMinsByEmpCode(EmpCode)
        .subscribe(GroupMis => {
          if (GroupMis) {
            this.InfoValue.GroupMIS = GroupMis.GroupMis;
            this.InfoValue.GroupMISString = GroupMis.GroupDesc;
            // Patch data to form
            this.InfoValueForm.patchValue({
              GroupMIS: this.InfoValue.GroupMIS,
              GroupMISString: this.InfoValue.GroupMISString,
            });
          }
        })
    }
  }

  // On BoMLowLevelSelect
  OnDetailSelect(Item: { data?: RequireQcWelder, option: number }) {
    if (Item) {
      if (!Item.data) {
        this.indexItem = -1;
      } else {
        this.indexItem = this.InfoValue.RequireQcWelder.indexOf(Item.data);
      }

      if (Item.option === 1) {
        let detailInfoValue: RequireQcWelder;
        // IF Edit data
        if (Item.data) {
          detailInfoValue = Object.assign({}, Item.data);
        } else { // Else New data
          detailInfoValue = {
            RequireHasWelderId: 0,
          };
        }
        // Send to dialog BomLowLevel
        this.serviceDialogs.dialogInfoRequireQcWelder(this.viewContainerRef, { InfoValue: detailInfoValue, Option: true })
          .subscribe(complateData => {
            if (complateData) {
              if (this.indexItem > -1) {
                // remove item
                this.InfoValue.RequireQcWelder.splice(this.indexItem, 1);
              }

              // cloning an object
              this.InfoValue.RequireQcWelder.push(Object.assign({}, complateData));
              this.InfoValue.RequireQcWelder = this.InfoValue.RequireQcWelder.slice();
              // Update to form
              this.InfoValueForm.patchValue({
                RequireQcWelder: this.InfoValue.RequireQcWelder
              });
            }
          })
      }
      else if (Item.option === 0) // Remove
      {
        this.InfoValue.RequireQcWelder.splice(this.indexItem, 1);
        this.InfoValue.RequireQcWelder = this.InfoValue.RequireQcWelder.slice();
        // Update to form
        this.InfoValueForm.patchValue({
          RequireQcWelder: this.InfoValue.RequireQcWelder
        });
      }
    }
  }

  ////////////
  // Module //
  ////////////

  // get attact file
  getAttach(): void {
    if (this.forFail) {
      if (this.InfoValue && this.InfoValue.ParentRequireQcId > 0) {
        this.serviceRequreQc.getAttachFile(this.InfoValue.ParentRequireQcId)
          .subscribe(dbAttach => {
            this.attachFiles = dbAttach.slice();
          }, error => console.error(error));
      }
    } else {
      if (this.InfoValue && this.InfoValue.RequireQualityControlId > 0) {
        this.serviceRequreQc.getAttachFile(this.InfoValue.RequireQualityControlId)
          .subscribe(dbAttach => {
            this.attachFiles = dbAttach.slice();
          }, error => console.error(error));
      }
    }
  }

  // on Attach Update List
  onUpdateAttachResults(results: FileList): void {
    // debug here
    // console.log("File: ", results);
    this.InfoValue.AttachFile = results;
    // debug here
    // console.log("Att File: ", this.RequirePaintList.AttachFile);

    this.InfoValueForm.patchValue({
      AttachFile: this.InfoValue.AttachFile
    });
  }

  // on Attach delete file
  onDeleteAttachFile(attach: AttachFile): void {
    if (this.forFail) {
      return;
    }

    if (attach) {
      if (!this.InfoValue.RemoveAttach) {
        this.InfoValue.RemoveAttach = new Array;
      }
      // remove
      this.InfoValue.RemoveAttach.push(attach.AttachFileId);
      // debug here
      // console.log("Remove :",this.InfoValue.RemoveAttach);

      this.InfoValueForm.patchValue({
        RemoveAttach: this.InfoValue.RemoveAttach
      });
      let template: Array<AttachFile> =
        this.attachFiles.filter((e: AttachFile) => e.AttachFileId !== attach.AttachFileId);

      this.attachFiles = new Array();
      setTimeout(() => this.attachFiles = template.slice(), 50);
    }
  }

  // open file attach
  onOpenNewLink(link: string): void {
    if (link) {
      window.open(link, "_blank");
    }
  }
}
