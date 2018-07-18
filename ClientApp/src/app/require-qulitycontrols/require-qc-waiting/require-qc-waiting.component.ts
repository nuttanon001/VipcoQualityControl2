import { Component, OnInit, OnDestroy, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import { OptionRequireQc } from "../shared/option-require-qc.model";
// 3rd patry
import { Column, SelectItem, LazyLoadEvent } from "primeng/primeng";
// service
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { AuthService } from "../../core/auth/auth.service";
import { RequireQualityControlService } from "../shared/require-qc.service";
import { RequireQc } from "../shared/require-qc.model";

@Component({
  selector: 'app-require-qc-waiting',
  templateUrl: './require-qc-waiting.component.html',
  styleUrls: ['./require-qc-waiting.component.scss']
})
export class RequireQcWaitingComponent implements OnInit, OnDestroy {

  constructor(
    private service: RequireQualityControlService,
    private serviceDialogs: DialogsService,
    private serviceAuth: AuthService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  // Parameter
  // model
  columns: Array<any>;
  columnUpper: Array<any>;
  columnLower: Array<any>;
  requireQc: Array<any>;
  scrollHeight: string;
  subscription: Subscription;
  // time
  message: number = 0;
  count: number = 0;
  time: number = 300;
  totalRecords: number;
  // value
  status: number | undefined;
  ProjectString: string;
  schedule: OptionRequireQc;
  loadReport: boolean;
  RequireQualityControlId: number;
  // form
  reportForm: FormGroup;

  // called by Angular after jobcard-waiting component initialized
  ngOnInit(): void {
    if (window.innerWidth >= 1600) {
      this.scrollHeight = 75 + "vh";
    } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
      this.scrollHeight = 68 + "vh";
    } else {
      this.scrollHeight = 65 + "vh";
    }

    this.requireQc = new Array;
    this.buildForm();

    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("condition") || 0);
      if (key) {
        if (this.status) {
          this.status = key;
          this.buildForm();
          if (this.reportForm) {
            this.onValueChanged();
          }
        } else {
          this.status = key;
          this.buildForm();
        }
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
  buildForm(): void {
    this.schedule = {
      Status: this.status || 1,
    };

    this.reportForm = this.fb.group({
      Filter: [this.schedule.Filter],
      ProjectId: [this.schedule.ProjectId],
      ProjectString: [this.ProjectString],
      SDate: [this.schedule.SDate],
      EDate: [this.schedule.EDate],
      Status: [this.schedule.Status],
      Skip: [this.schedule.Skip],
      Take: [this.schedule.Take],
    });

    this.reportForm.valueChanges.debounceTime(350)
        .subscribe((data: any) => this.onValueChanged(data));
    // this.onValueChanged();
  }

  // on value change
  onValueChanged(data?: any): void {
    if (!this.reportForm) { return; }
    this.schedule = this.reportForm.value;
    this.onGetData(this.schedule);
  }

  // get request data
  onGetData(schedule: OptionRequireQc): void {
    this.service.getRequireQualityControlWaiting(schedule)
      .subscribe(dbDataSchedule => {
        // console.log("Api Send is", dbDataSchedule);
        if (!dbDataSchedule) {
          this.totalRecords = 0;
          this.columns = new Array;
          this.columnLower = new Array;
          this.columnUpper = new Array;
          this.requireQc = new Array;
          this.reloadData();
          return;
        }
        this.totalRecords = dbDataSchedule.TotalRow;
        // new Column Array
        this.columnUpper = new Array;
        this.columns = new Array;
        // Width
        let Width100: string = "100px";
        let Width125: string = "125px";
        let Width150: string = "150px";
        let MainGroupName = (this.status === 1 ? "WorkGroup-QC" : "WeldingData");
        // column Row1
        this.columnUpper.push({ header: MainGroupName, rowspan: 2, style: { "width": Width150, } });
        for (let detail of dbDataSchedule.ColumnUpper) {
          // debug here
          //console.log("detail", JSON.stringify(detail));
          this.columnUpper.push({
            header: detail,
            colspan: 2,
            style: { "width": (2 * 150).toString() + "px", }
          });
        }
        // debug here
        // console.log("columnUpper", JSON.stringify(this.columnUpper));
        // column Row2
        this.columnLower = new Array;
        for (let name of dbDataSchedule.ColumnLower) {
          this.columnLower.push({
            header: name,
          });
        }

        this.columns.push({
          header: MainGroupName, field: "WorkGroupQcName",
          style: { "width": Width150 }, styleclass: "time-col",
        });

        let i: number = 0;
        for (let name of dbDataSchedule.ColumnAll) {
          if (name.indexOf("Col") >= -1) {
            this.columns.push({
              header: this.columnLower[i], field: name, style: { "width": Width150 }, isCol: true,
            });
            i++;
          }
        }

        this.requireQc = dbDataSchedule.DataTable.slice();
        // console.log("OverTime is:", this.overtimeMasters);
        this.reloadData();
      }, error => {
        this.totalRecords = 0;
        this.columns = new Array;
        this.columnLower = new Array;
        this.columnUpper = new Array;
        this.requireQc = new Array;
        this.reloadData();
      });
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
            this.onGetData(this.reportForm.value);
          }
        }
      });
  }

  // open dialog
  openDialog(type?: string): void {
    if (type) {
      if (type === "Project") {
        this.serviceDialogs.dialogSelectProject(this.viewContainerRef,2)
          .subscribe(project => {
            if (project) {
              this.reportForm.patchValue({
                ProjectId: project.ProjectCodeSub.ProjectCodeDetailId,
                ProjectString: `${project.ProjectCode}/${project.ProjectCodeSub.ProjectCodeDetailCode}`,
              });
            }
          });
      }
    }
  }

  // reset
  resetFilter(): void {
    this.requireQc = new Array;
    this.buildForm();
    this.onGetData(this.schedule);
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
    this.reportForm.patchValue({
      Skip: event.first,
      Take: (event.rows || 20),
    });
  }

  // on selected data
  onSelectRow(master?: RequireQc): void {
    if (master) {
      if (this.status === 1) {
        this.serviceDialogs.dialogSelectRequireQualityControl(master.RequireQualityControlId, this.viewContainerRef)
          .subscribe(conditionNumber => {
            if (conditionNumber) {
              if (conditionNumber === -1) {
                this.onUpdateRequireMaintenance(master.RequireQualityControlId);
                setTimeout(() => { this.onGetData(this.reportForm.value); }, 750);
              } else if (conditionNumber === 2) {
                setTimeout(() => { this.onGetData(this.reportForm.value); }, 750);
              } else if (conditionNumber === 1) {
                this.router.navigate(["qualitycontrol/", master.RequireQualityControlId]);
              } else if (conditionNumber === 3) {
                this.RequireQualityControlId = master.RequireQualityControlId;
                this.loadReport = !this.loadReport;
              } else if (conditionNumber === -2) {
                this.serviceDialogs.confirm("Question Message", "Do you want to cancel requrie quality control?",
                  this.viewContainerRef).subscribe(result => {
                    if (result) {
                      this.service.cancelRequireQualityControl(master.RequireQualityControlId)
                        .subscribe(dbData => {
                          setTimeout(() => { this.onGetData(this.reportForm.value); }, 750);
                        });
                    }
                  });
              }
            }
          });
      }
      // For Welding Workgroup
      else if (this.status === 2) {
        this.serviceDialogs.dialogSelectRequireQualityControl(master.RequireQualityControlId, this.viewContainerRef)
          .subscribe(conditionNumber => {
            if (conditionNumber) {
              if (conditionNumber === 4) {
                setTimeout(() => {
                  this.router.navigate(["require-qc-welder/", master.RequireQualityControlId]);
                }, 750);
              } else if (conditionNumber === -2) {
                this.serviceDialogs.confirm("Question Message", "Do you want to cancel requrie quality control?",
                  this.viewContainerRef).subscribe(result => {
                    if (result) {
                      this.service.cancelRequireQualityControl(master.RequireQualityControlId)
                        .subscribe(dbData => {
                          setTimeout(() => { this.onGetData(this.reportForm.value); }, 750);
                        });
                    }
                  });
              }
            }
          });
        // TODO
      }
    }
  }

  // RequireMaintenance Has Action
  onUpdateRequireMaintenance(RequireQcId: number): void {
    this.service.actionRequireQualityControl(RequireQcId, (this.serviceAuth.getAuth.UserName || ""))
        .subscribe();
  }

  // on back from report
  onBack(): void {
    this.loadReport = !this.loadReport;
  }
}
