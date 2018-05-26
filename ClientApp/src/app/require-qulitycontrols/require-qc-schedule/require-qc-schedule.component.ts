import { Component, OnInit, OnDestroy, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import { RequireQcSchedule } from "../shared/require-qc-schedule.model";
import { WorkGroupQc } from "../../workgroup-qulitycontrols/shared/workgroup-qc.model";
// 3rd patry
import { Column, SelectItem, LazyLoadEvent } from "primeng/primeng";
// service
import { AuthService } from "../../core/auth/auth.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { RequireQualityControlService } from "../shared/require-qc.service";
import { WorkGroupQcService } from "../../workgroup-qulitycontrols/shared/workgroup-qc.service";

@Component({
  selector: 'app-require-qc-schedule',
  templateUrl: './require-qc-schedule.component.html',
  styleUrls: ['./require-qc-schedule.component.scss']
})
export class RequireQcScheduleComponent implements OnInit, OnDestroy {
  /** paint-task-schedule ctor */
  constructor(
    private service: RequireQualityControlService,
    private serviceWorkGroupQc: WorkGroupQcService,
    private serviceDialogs: DialogsService,
    private serviceAuth: AuthService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder,
    private router: Router,
    public route: ActivatedRoute) { }

  // Parameter
  // form
  reportForm: FormGroup;
  // model
  columnsUpper: Array<any>;
  columnsLower: Array<any>;
  columnsLowerLv2: Array<any>;
  columns: Array<any>;
  requireQualityControls: Array<any>;
  // subscription
  subscription: Subscription;
  // time
  message: number = 0;
  count: number = 0;
  time: number = 300;
  totalRecords: number;
  // mode
  mode: number | undefined;
  schedule: RequireQcSchedule;
  requireQualityControlId: number;
  // report
  isLinkMail: boolean = false;
  loadReport: boolean;
  ReportType?: string;
  // array data
  workGroupQcs: Array<WorkGroupQc>;

  // angular hook
  ngOnInit(): void {
    this.loadReport = false;
    this.ReportType = "";

    this.requireQualityControls = new Array;
    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("condition") || 0);
      if (key) {
        this.mode = 2;
        let schedule: RequireQcSchedule = {
          RequireQuailtyControlId: key
        };

        this.buildForm(schedule);

        //if (this.reportForm) {
        //  this.onValueChanged();
        //}
      }
    }, error => console.error(error));
  }

  // destroy
  ngOnDestroy(): void {
    if (this.subscription) {
      // prevent memory leak when component destroyed
      this.subscription.unsubscribe();
    }
  }

  // build form
  buildForm(schedule?: RequireQcSchedule): void {
    if (!schedule) {
      schedule = {
        Mode: this.mode || 2,
      };
    }
    this.schedule = schedule;

    this.reportForm = this.fb.group({
      Filter: [this.schedule.Filter],
      ProjectMasterId: [this.schedule.ProjectMasterId],
      ProjectMasterString: [this.schedule.ProjectMasterString],
      Mode: [this.schedule.Mode],
      Skip: [this.schedule.Skip],
      Take: [this.schedule.Take],
      QuailtyControlId: [this.schedule.QuailtyControlId],
      RequireQuailtyControlId: [this.schedule.RequireQuailtyControlId],
      WorkGroupQuailtyControlId: [this.schedule.WorkGroupQuailtyControlId],
      Creator: [this.schedule.Creator],
      SDate: [this.schedule.SDate],
      EDate: [this.schedule.EDate],
      // template
      CreatorName: [this.schedule.CreatorName],
    });
    this.reportForm.valueChanges.debounceTime(350)
        .subscribe((data: any) => this.onValueChanged(data));
    // this.onValueChanged();
    this.getWorkGroupQuailtyControl();
  }

  // on value change
  onValueChanged(data?: any): void {
    if (!this.reportForm) { return; }

    this.schedule = this.reportForm.value;
    this.onGetTaskMasterSchedule(this.schedule);
  }

  // get work group quailty control
  getWorkGroupQuailtyControl(): void {
    if (!this.workGroupQcs) {
      this.workGroupQcs = new Array;
    }
    this.serviceWorkGroupQc.getAll()
      .subscribe(dbData => {
        this.workGroupQcs = dbData.slice();
      });
  }

  // get task master schedule data
  onGetTaskMasterSchedule(schedule: RequireQcSchedule): void {
    if (this.requireQualityControlId) {
      schedule.RequireQuailtyControlId = this.requireQualityControlId;
    }

    this.service.getRequireQualityControlSchedule2(schedule)
      .subscribe(dbDataSchedule => {
        this.onSetupDataTable(dbDataSchedule);
      }, error => {
        this.totalRecords = 0;
        this.columns = new Array;
        this.requireQualityControls = new Array;
        this.reloadData();
      });
  }

  // on setup datatable
  onSetupDataTable(dbDataSchedule: any): void {
    if (!dbDataSchedule || dbDataSchedule.length < 1) {
      this.columns = new Array;
      this.requireQualityControls = new Array;
      this.reloadData();
      return;
    }

    this.totalRecords = dbDataSchedule.TotalRow;

    this.columns = new Array;
    this.columnsUpper = new Array;

    let Width100: string = "100px";
    let Width125: string = "125px";
    let Width150: string = "150px";

    // column Row1
    this.columnsUpper.push({ header: "JobNumber", rowspan: 3, style: { "width": Width150, } });
    this.columnsUpper.push({ header: "WorkGroupQC", rowspan: 3, style: { "width": Width150, } });
    this.columnsUpper.push({ header: "Inspection", rowspan: 3, style: { "width": Width125, } });
    this.columnsUpper.push({ header: "Progress", rowspan: 3, style: { "width": Width100, } });
    // this.columnsUpper.push({ header: "QualityControlStatus", rowspan: 2, style: { "width": Width100, } });

    for (let month of dbDataSchedule.ColumnsTop) {
      this.columnsUpper.push({
        header: month.Name,
        colspan: month.Value,
        style: { "width": (month.Value * 50).toString() + "px", }
      });
    }
    // column Row2
    this.columnsLower = new Array;

    for (let name of dbDataSchedule.ColumnsLow) {
      this.columnsLower.push({
        header: name,
        colspan: 2,
        // style: { "width": "25px" }
      });
    }

    // column Row3
    this.columnsLowerLv2 = new Array;
    for (let name of dbDataSchedule.ColumnsLower) {
      this.columnsLowerLv2.push({
        header: name,
        // style: { "width": "25px" }
      });
    }

    // column Main
    this.columns.push({
      header: "JobNo",
      field: "ProjectMaster",
      style: { "width": Width150, }
    });
    this.columns.push({
      header: "WorkGroupQC",
      field: "WorkGroupQualityControl",
      style: { "width": Width150, },
    });
    this.columns.push({
      header: "Inspection",
      field: "InspectionPoint",
      style: { "width": Width125, },
      isLink: true
    });
    this.columns.push({
      header: "Progress",
      field: "Progress",
      style: { "width": Width100, }
    });

    let i: number = 0;
    for (let name of dbDataSchedule.ColumnsAll) {
      if (name.indexOf("Col") >= -1) {
        this.columns.push({
          header: this.columnsLower[i], field: name, style: { "width": "50px" }, isCol: true,
        });
        i++;
      }
    }

    this.requireQualityControls = dbDataSchedule.DataTable.slice();
    this.reloadData();
  }

  // reload data
  reloadData(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.subscription = Observable.interval(1000)
      .take(this.time).map((x) => x + 1)
      .subscribe((x) => {
        this.message = this.time - x;
        this.count = (x / this.time) * 100;
        if (x === this.time) {
          if (this.reportForm.value) {
            ;
            this.onGetTaskMasterSchedule(this.reportForm.value);
          }
        }
      });
  }

  // reset
  resetFilter(): void {
    this.requireQualityControls = new Array;
    this.buildForm();

    this.reportForm.patchValue({
      Skip: 0,
      Take: 10,
    });

    // this.onGetTaskMasterSchedule(this.reportForm.value);
  }

  // load Data Lazy
  loadDataLazy(event: LazyLoadEvent): void {
    // in a real application, make a remote request to load data using state metadata from event
    // event.first = First row offset
    // event.rows = Number of rows per page
    // event.sortField = Field name to sort with
    // event.sortOrder = Sort order as number, 1 for asc and -1 for dec
    // filters: FilterMetadata object having field as key and filter value, filter matchMode as value

    // imitate db connection over a network
    if (!this.reportForm) {
        this.buildForm({
          Creator: this.serviceAuth.getAuth ? this.serviceAuth.getAuth.EmpCode : undefined,
          CreatorName: this.serviceAuth.getAuth ? this.serviceAuth.getAuth.NameThai : undefined,
          Mode: 2,
        });
    }
    this.reportForm.patchValue({
      Skip: event.first,
      // mark Take: ((event.first || 0) + (event.rows || 4)),
      Take: (event.rows || 10),
    });
  }

  // on select dialog
  onShowDialog(type?: string): void {
    if (type) {
      if (type === "Employee") {
        this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
          .subscribe(emp => {
            if (emp) {
              this.reportForm.patchValue({
                Creator: emp.EmpCode,
                CreatorName: `คุณ${emp.NameThai}`,
              });
            }
          });
      } else if (type === "Project") {
        this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
          .subscribe(project => {
            if (project) {
              this.reportForm.patchValue({
                ProjectMasterId: project.ProjectCodeMasterId,
                ProjectMasterString: `${project.ProjectCode}/${project.ProjectName}`,
              });
            }
          });
      }
    }
  }

  // on update progress
  onSelectRequireQualityControl(QualityControlId?: number): void {
    if (QualityControlId) {
      // On Schedule readonly show dialog
      this.serviceDialogs.dialogSelectQualityControl(QualityControlId, this.viewContainerRef);
    } else {
      this.serviceDialogs.error("Warning Message", "This request quality control not plan yet.", this.viewContainerRef);
    }
  }
}
