// angular
import { Component, ViewContainerRef, Input } from "@angular/core";
import { FormBuilder, FormControl, Validators, AbstractControl } from "@angular/forms";
// models
import { RequireQc } from "../shared/require-qc.model";
import { Branch } from "../../branchs/shared/branch.model";
import { AttachFile } from "../../shared/attach-file.model";
import { RequireStatusQc } from "../shared/require-status-qc.enum";
import { MasterList } from "../../master-lists/shared/master-list.model";
import { WorkActivity } from "../../work-activities/shared/work-activity.model";
import { WorkGroupQc } from "../../workgroup-qulitycontrols/shared/workgroup-qc.model";
import { InspectionPoint } from "../../inspection-points/shared/inspection-point.model";
// components
import { BaseEditComponent } from "../../shared/base-edit-component";
// services
import { AuthService } from "../../core/auth/auth.service";
import { ShareService } from "../../shared/share.service";
import { BranchService } from "../../branchs/shared/branch.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { WorkActivityService } from "../../work-activities/shared/work-activity.service";
import { EmployeeGroupMisService } from "../../employees/shared/employee-group-mis.service";
import { WorkGroupQcService } from "../../workgroup-qulitycontrols/shared/workgroup-qc.service";
import { InspectionPointService } from "../../inspection-points/shared/inspection-point.service";
import { RequireQualityControlCommunicateService, RequireQualityControlService } from "../shared/require-qc.service";
import { MasterListService } from "../../master-lists/shared/master-list.service";
import { RequireMoreWorkactivityService } from "../shared/require-more-workactivity.service";
// Rxjs
import { filter } from "rxjs/operator/filter";
import { debounceTime, distinctUntilChanged, switchMap } from "rxjs/operators";
import { TypeworkActivity } from "../../work-activities/shared/typework-activity.enum";

@Component({
  selector: 'app-require-qc-edit',
  templateUrl: './require-qc-edit.component.html',
  styleUrls: ['./require-qc-edit.component.scss']
})
export class RequireQcEditComponent extends BaseEditComponent<RequireQc, RequireQualityControlService> {
  constructor(
    service: RequireQualityControlService,
    serviceCom: RequireQualityControlCommunicateService,
    private serviceRequireMoreActivity: RequireMoreWorkactivityService,
    private serviceBranch: BranchService,
    private serviceMarkNo: MasterListService,
    private serviceGroupMis: EmployeeGroupMisService,
    private serviceWorkActivity: WorkActivityService,
    private serviceWorkGroupQc: WorkGroupQcService,
    private serviceInspection: InspectionPointService,
    private serviceAuth: AuthService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder
  ) {
    super(
      service,
      serviceCom,
    );
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
  
  // on get data by key
  onGetDataByKey(value?: RequireQc): void {
    if (value) {
      if (!value.ParentRequireQcId) {
        this.service.getOneKeyNumber(value)
          .subscribe(dbData => {
            this.editValue = dbData;
            this.editValue.RequireQcTime = dbData.RequireQcTimeString;
            //this.editValue.RequireQcTime = new Date();
            // Debug
            //console.log(dbData.RequireDate.getTime());
            //this.editValue.RequireQcTime.setTime(dbData.RequireDate.getTime());
            //console.log("Dbdata", JSON.stringify(this.editValue));

            //RequireQualityControl
            this.serviceMarkNo.actionRequireQualityControlHasMarkNo(dbData.RequireQualityControlId)
              .subscribe(RequireQCHasMasterList => {
                this.editValue.MasterLists = new Array;
                if (RequireQCHasMasterList) {
                  RequireQCHasMasterList.forEach((item, index) => {
                    this.editValue.MasterLists.push({
                      CreateDate: item.CreateDate,
                      Creator: item.Creator,
                      MarkNo: item.MarkNo,
                      Name: item.Name,
                      UnitNo: item.UnitNo,
                      DrawingNo: item.DrawingNo,
                      Box: item.Box,
                      Quantity: item.Quantity,
                      MasterProjectListId: item.MasterProjectListId
                    });
                  });
                  //Patch value to Form
                  this.editValueForm.patchValue({
                    MasterLists: this.editValue.MasterLists
                  });
                }
              });
          }, error => console.error(error), () => this.buildForm());
      } else { // Form fail require quality control
        this.forFail = true;

        this.editValue = value;
        this.editValue.RequireDate = new Date;
        // this.editValue.RequireQcTimeString = (new Date).toLocaleTimeString("th-TH", { hour12: false });
        this.editValue.RequireQcTimeString = (new Date(this.editValue.RequireDate.getTime() + 60 * 60000)).toLocaleTimeString("th-TH", { hour12: false });
        this.editValue.RequireQcTime = this.editValue.RequireQcTimeString;
        this.buildForm();

      }
    } else {
      this.editValue = {
        RequireQualityControlId: 0,
        RequireDate: new Date,
        RequireStatus: RequireStatusQc.Waiting,
      };
      // TimeString
      this.editValue.RequireQcTimeString = (new Date(this.editValue.RequireDate.getTime() + 60 * 60000)).toLocaleTimeString("th-TH", { hour12: false });
      this.editValue.RequireQcTime = this.editValue.RequireQcTimeString;

      if (this.serviceAuth.getAuth) {
        this.editValue.RequireEmp = this.serviceAuth.getAuth.EmpCode;
        this.editValue.RequireEmpString = this.serviceAuth.getAuth.NameThai;
        this.editValue.MailReply = this.serviceAuth.getAuth.MailAddress;
        // Get GroupMIS
        // this.getEmployeeGroupMisByEmpCode(this.editValue.RequireEmp);
      }
      this.buildForm();
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
    this.editValueForm = this.fb.group({
      RequireQualityControlId: [this.editValue.RequireQualityControlId],
      RequireQualityNo: [this.editValue.RequireQualityNo],
      RequireDate: [this.editValue.RequireDate,
        [
          Validators.required,
        ]
      ],
      ResponseDate: [this.editValue.ResponseDate],
      Description: [this.editValue.Description,
        [
          Validators.maxLength(250)
        ]
      ],
      Remark: [this.editValue.Remark,
        [
          Validators.maxLength(250)
        ]
      ],
      MailReply: [this.editValue.MailReply],
      RequireStatus: [this.editValue.RequireStatus],
      //FK
      ParentRequireQcId: [this.editValue.ParentRequireQcId],
      GroupMIS: [this.editValue.GroupMIS],
      RequireEmp: [this.editValue.RequireEmp],
      ProjectCodeDetailId: [this.editValue.ProjectCodeDetailId],
      WorkGroupQualityControlId: [this.editValue.WorkGroupQualityControlId,
        [
          Validators.required
        ]
      ],
      InspectionPointId: [this.editValue.InspectionPointId,
        [
          Validators.required
        ]
      ],
      WorkActivityId: [this.editValue.WorkActivityId],
      BranchId: [this.editValue.BranchId,
        [
          Validators.required
        ]
      ],
      // BaseModel
      Creator: [this.editValue.Creator],
      CreateDate: [this.editValue.CreateDate],
      Modifyer: [this.editValue.Modifyer],
      ModifyDate: [this.editValue.ModifyDate],
      // ViewModel
      GroupMISString: [this.editValue.GroupMISString,
        [
          Validators.required
        ]
      ],
      RequireEmpString: [this.editValue.RequireEmpString,
        [
          Validators.required
        ]
      ],
      ProjectCodeDetailString: [this.editValue.ProjectCodeDetailString,
        [
          Validators.required
        ]
      ],
      WorkGroupQualityControlString: [this.editValue.WorkGroupQualityControlString],
      InspectionPointString: [this.editValue.InspectionPointString],
      WorkActivityString: [this.editValue.WorkActivityString],
      BranchString: [this.editValue.BranchString],
      RequireStatusString: [this.editValue.RequireStatusString],
      MasterLists: [this.editValue.MasterLists],
      RequireQcTime: [this.editValue.RequireQcTime],
      MoreWorkActvities: [this.editValue.MoreWorkActvities],
      // Attach File
      AttachFile: [this.editValue.AttachFile],
      RemoveAttach: [this.editValue.RemoveAttach],
    });
    this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));

    const ControlMoreActivities: AbstractControl | undefined = this.editValueForm.get("MoreWorkActvities");
    if (ControlMoreActivities) {
      ControlMoreActivities.valueChanges
        .pipe(
          debounceTime(500),
          distinctUntilChanged()).subscribe((data: Array<WorkActivity>) => {
            if (data) {
              let isNde = data.find((work) => work.TypeWorkActivity === TypeworkActivity.NDE);
              let isQc = data.find((work) => work.TypeWorkActivity === TypeworkActivity.QC_QA);

              if (isNde && isQc) {
                this.serviceDialogs.error("Warning Message",
                  "Cannot select both type of work-activity.", this.viewContainerRef)
                  .subscribe(result => {
                    //Patch value to Form
                    this.editValueForm.patchValue({
                      MoreWorkActvities: undefined
                    });
                  });
              }
            }
          });
    }
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

          if (!this.editValue.BranchId) {
            this.editValue.BranchId = this.branchs.find(item => item.Name.toLowerCase() === "vipco2").BranchId || undefined;
            this.editValueForm.patchValue({
              BranchId: this.editValue.BranchId
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
          this.workActivities = [...dbWorkActivity];
          //dbWorkActivity.forEach((item) => {
          //  this.workActivities.push({
          //    CreateDate: item.CreateDate,
          //    Creator: item.Creator,
          //    Description: item.Description,
          //    Name: item.Name,
          //    Remark: item.Remark,
          //    TypeWorkActivity: item.TypeWorkActivity,
          //    WorkActivityId:item.WorkActivityId
          //  })
          //});

          //debug here
          // console.log("Data is", JSON.stringify(this.workActivities));

          let id: number = this.editValue.RequireQualityControlId || this.editValue.ParentRequireQcId;
          if (id) {
            this.serviceRequireMoreActivity.getByMasterId(id)
              .subscribe(dbData => {
                if (!this.editValue.MoreWorkActvities) {
                  this.editValue.MoreWorkActvities = new Array;
                }

                if (dbData) {
                  dbData.forEach((item) => {
                    let copy = this.workActivities.find((work) => work.WorkActivityId == item.WorkActivityId);
                    // console.log(JSON.stringify(copy));
                    if (copy) {
                      this.editValue.MoreWorkActvities.push(copy);
                    }
                  });
                  // debug here
                  // console.log("Data is", JSON.stringify(this.editValue.MoreWorkActvities));
                  //Patch value to Form
                  this.editValueForm.patchValue({
                    MoreWorkActvities: this.editValue.MoreWorkActvities
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
            this.editValue.GroupMIS = GroupMis.GroupMis;
            this.editValue.GroupMISString = GroupMis.GroupDesc;
            // Patch data to form
            this.editValueForm.patchValue({
              GroupMIS: this.editValue.GroupMIS,
              GroupMISString: this.editValue.GroupMISString,
            });
          }
        })
    }
  }

  // open dialog
  openDialog(type?: string): void {
    if (this.forFail) {
      return;
    }

    if (type) {
      if (type === "Employee") {
        this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
          .subscribe(emp => {
            if (emp) {
              this.editValueForm.patchValue({
                RequireEmp: emp.EmpCode,
                RequireEmpString: `คุณ${emp.NameThai}`,
              });

              this.getEmployeeGroupMisByEmpCode(emp.EmpCode);
            }
          });
      } else if (type === "Project") {
        this.serviceDialogs.dialogSelectProject(this.viewContainerRef, 2)
          .subscribe(project => {
            if (project) {
              if (project.ProjectCodeSub) {
                this.editValueForm.patchValue({
                  ProjectCodeDetailId: project.ProjectCodeSub.ProjectCodeDetailId,
                  ProjectCodeDetailString: `${project.ProjectCode}/${project.ProjectCodeSub.ProjectCodeDetailCode}`,
                });
              }
            }
          });
      } else if (type === "GroupMis") {
        this.serviceDialogs.dialogSelectGroupMis(this.viewContainerRef)
          .subscribe(groupMis => {
            if (groupMis) {
              this.editValueForm.patchValue({
                GroupMIS: groupMis.GroupMis,
                GroupMISString: `${groupMis.GroupDesc}/${groupMis.LocaltionString || "-"}`,
              });
            }
          });
      } else if (type === "Location") {
        this.serviceDialogs.dialogSelectLocationAndCreateLocation(this.viewContainerRef, 0)
          .subscribe(location => {
            if (location) {
              this.editValueForm.patchValue({
                LocationQualityControlId: location.LocationQualityControlId,
                LocationQualityControlString: location.Name,
              });
            }
          });
      }
    }
  }

  // action markno
  actionMarkNo(markNo?: { data: MasterList, option: number }): void {
    if (!this.editValue.MasterLists) {
      this.editValue.MasterLists = new Array;
    }

    let dataMarkNo: MasterList = {
      MasterProjectListId : 0
    };

    if (!markNo) {
      this.indexItem = -1;
    } else {
      this.indexItem = this.editValue.MasterLists.indexOf(markNo.data);

      if (markNo.option === 1) {
        dataMarkNo = Object.assign({}, markNo.data);
      } else {
        this.editValue.MasterLists.splice(this.indexItem, 1);
        this.editValue.MasterLists = this.editValue.MasterLists.slice();
        // Update to form
        this.editValueForm.patchValue({
          MasterLists: this.editValue.MasterLists
        });
        return;
      }
    }

    this.serviceDialogs.dialogCreateMarkNo(this.viewContainerRef, dataMarkNo)
      .subscribe(dialogMarkNo => {
        if (dialogMarkNo) {
          if (this.indexItem > -1) {
            // remove item
            this.editValue.MasterLists.splice(this.indexItem, 1);
          }
          // cloning an object
          this.editValue.MasterLists.push(Object.assign({}, dialogMarkNo));
          this.editValue.MasterLists = this.editValue.MasterLists.slice();
          // Update to form
          this.editValueForm.patchValue({
            MasterLists: this.editValue.MasterLists
          });
          this.onValueChanged();
        } else {
          this.onValueChanged();
        }
      });
  }

  //////////////
  // Override //
  //////////////

  // Over ride on valid data
  onFormValid(isValid: boolean): void {
    //Debug
    // console.log("ValueForm", JSON.stringify(this.editValueForm.value));
    this.editValue = this.editValueForm.value;
    //Debug
    // console.log(this.editValue);
    // console.log("Value", JSON.stringify(this.editValue));

    if (this.editValue) {
      if (!this.editValue.MasterLists) {
        isValid = false;
      } else if (this.editValue.MasterLists.length < 1) {
        isValid = false;
      } 
    }

    if (this.editValue.MoreWorkActvities) {
      this.editValue.MoreWorkActvities.forEach((item, index) => {
        if (this.editValue.MoreWorkActvities) {
          let newData: WorkActivity = {
            CreateDate: item.CreateDate,
            Creator: item.Creator,
            Description: item.Description,
            Name: item.Name,
            Remark: item.Remark,
            TypeWorkActivity: item.TypeWorkActivity,
            WorkActivityId: item.WorkActivityId,
          };

          this.editValue.MoreWorkActvities[index] = newData;
        }
      });
    }

    this.communicateService.toParent([this.editValue, isValid]);
  }

  // On Angular Init
  ngOnInit(): void {
    this.forFail = false;
    // ngOnInit
    super.ngOnInit();
  }

  ////////////
  // Module //
  ////////////

  // get attact file
  getAttach(): void {
    if (this.forFail) {
      if (this.editValue && this.editValue.ParentRequireQcId > 0) {
        this.service.getAttachFile(this.editValue.ParentRequireQcId)
          .subscribe(dbAttach => {
            this.attachFiles = dbAttach.slice();
          }, error => console.error(error));
      }
    } else {
      if (this.editValue && this.editValue.RequireQualityControlId > 0) {
        this.service.getAttachFile(this.editValue.RequireQualityControlId)
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
    this.editValue.AttachFile = results;
    // debug here
    // console.log("Att File: ", this.RequirePaintList.AttachFile);

    this.editValueForm.patchValue({
      AttachFile: this.editValue.AttachFile
    });
  }

  // on Attach delete file
  onDeleteAttachFile(attach: AttachFile): void {
    if (this.forFail) {
      return;
    }

    if (attach) {
      if (!this.editValue.RemoveAttach) {
        this.editValue.RemoveAttach = new Array;
      }
      // remove
      this.editValue.RemoveAttach.push(attach.AttachFileId);
      // debug here
      // console.log("Remove :",this.editValue.RemoveAttach);

      this.editValueForm.patchValue({
        RemoveAttach: this.editValue.RemoveAttach
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
