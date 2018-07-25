import { Component, OnInit, ViewContainerRef, ViewChild } from '@angular/core';
import { Location } from "@angular/common";
import { BaseMasterComponent } from '../../shared/mk2/base-master-component';
import { RequireQcWelder } from '../shared/require-qc-welder.model';
import { RequireQcWelderService } from '../shared/require-qc-welder.service';
import { RequireQcWelderCommunicateService } from '../shared/require-qc-welder-communicate.service';
import { AuthService } from '../../core/auth/auth.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { RequireQcWelderTableComponent } from '../require-qc-welder-table/require-qc-welder-table.component';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';

@Component({
  selector: 'app-require-qc-welder-master',
  templateUrl: './require-qc-welder-master.component.html',
  styleUrls: ['./require-qc-welder-master.component.scss']
})
export class RequireQcWelderMasterComponent extends BaseMasterComponent<RequireQc, RequireQcWelderService, RequireQcWelderCommunicateService> {
  constructor(
    service: RequireQcWelderService,
    serviceCom: RequireQcWelderCommunicateService,
    serviceAuth: AuthService,
    serviceDia: DialogsService,
    viewConRef: ViewContainerRef,
    private serviceRequireQc: RequireQualityControlService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
  ) { super(service, serviceCom, serviceAuth, serviceDia, viewConRef); }
  //Parameter
  backToSchedule: boolean = false;
  loadReport: boolean = false;
  forFail: boolean = false;
  @ViewChild(RequireQcWelderTableComponent)
  private tableComponent: RequireQcWelderTableComponent;

  //on ngInitAngular
  ngOnInit(): void {
    super.ngOnInit();
    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("key") || 0);
      if (key) {
        this.backToSchedule = true;
        let value : { data: RequireQc, option: number } = {
          data: { RequireQualityControlId : key },
          option: 1
        }
        this.onDetailView(value);
      }
    }, error => console.error(error));

    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("condition") || 0);
      if (key) {
        this.forFail = true;
        // setTimeout(() => {
        //  this.onReloadData();
        // }, 500);
      }
    }, error => console.error(error));

    this.route.paramMap.subscribe((param: ParamMap) => {
      let key: number = Number(param.get("report") || 0);
      if (key) {
        this.displayValue = { RequireQualityControlId: key };
        this.loadReport = true;
        this.backToSchedule = true;
      }
    }, error => console.error(error));
  }

  //on Reload
  onReloadData(): void {
    this.tableComponent.reloadData();
  }

  // on info view
  onInfoView(value?: RequireQc): void {
    this.displayValue = value;
  }

  // on detail view
  onDetailView(value?: { data: RequireQc, option: number }): void {
    if (value) {
      if (value.option === 1) {
        this.onLoading = true;
        if (this.forFail) {
          this.service.getGenarateFromFailRequireQualityControl(value.data.RequireQualityControlId)
            .subscribe(dbData => {
              this.displayValue = dbData;
              this.ShowDetail = true;
              setTimeout(() => {
                this.communicateService.toChildEdit(this.displayValue);
                this.onLoading = false;
              }, 1000);
            });
        } else {
          this.displayValue = value.data;
          this.ShowDetail = true;
          setTimeout(() => {
            this.communicateService.toChildEdit(this.displayValue);
            this.onLoading = false;
          }, 1000);
        }
      } else if (value.option === 0) {
        if (!this.authService) { return; }
        if (this.authService.getAuth.LevelUser < 3) {
          this.dialogsService.error("Access Deny", "Access is restricted", this.viewContainerRef).subscribe();
          return;
        }
        this.onLoading = true;
        this.dialogsService.confirm("Warning Message", "Do you want delete this item?", this.viewContainerRef)
          .subscribe(result => {
            if (result) {
              this.service.deleteKeyNumber(value.data)
                .subscribe(dbData => { }, error => { }, () => {
                  this.onLoading = false;
                  this.onReloadData();
                });
            } else {
              this.onLoading = false;
            }
          });
        this.canSave = false;
      }
    } else {
      this.displayValue = undefined;
      this.ShowDetail = true;
      setTimeout(() => this.communicateService.toChildEdit(this.displayValue), 1000);
    }
  }

  // on insert data
  onInsertToDataBase(value: RequireQc): void {
    this.onLoading = true;
    if (this.authService.getAuth) {
      value["Creator"] = this.authService.getAuth.UserName || "";
    }
    let attachs: FileList | undefined = value.AttachFile;

    // insert data
    this.service.addModel(value).subscribe(
      (complete: any) => {
        if (complete && attachs) {
          this.onAttactFileToDataBase(complete.RequireQualityControlId, attachs, complete.Creator || "");
        }
        if (complete) {
          this.displayValue = complete;
          this.onSaveComplete();
        }
        if (this.onLoading) {
          this.onLoading = false;
        }
      },
      (error: any) => {
        console.error(error);
        this.dialogsService.error("Failed !",
          "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
      }
    );
  }

  // on update data
  onUpdateToDataBase(value: RequireQc): void {
    this.onLoading = true;
    if (this.authService.getAuth) {
      value["Modifyer"] = this.authService.getAuth.UserName || "";
    }

    let attachs: FileList | undefined = value.AttachFile;
    // remove attach
    if (value.RemoveAttach) {
      this.onRemoveFileFromDataBase(value.RemoveAttach);
    }

    // update data
    this.service.updateModelWithKey(value).subscribe(
      (complete: any) => {
        if (complete && attachs) {
          this.onAttactFileToDataBase(complete.RequireQualityControlId, attachs, complete.Modifyer || "Someone");
        }

        if (complete) {
          this.displayValue = complete;
          this.onSaveComplete();
        }
        if (this.onLoading) {
          this.onLoading = false;
        }
      },
      (error: any) => {
        console.error(error);
        this.dialogsService.error("Failed !",
          "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
      }
    );
  }

  // on save complete
  onSaveComplete(): void {
    this.dialogsService.context("System message", "Save completed.", this.viewContainerRef)
      .subscribe(result => {
        this.ShowDetail = false;
        this.displayValue = undefined;
        this.onBack();
      });
  }
  
  ////////////
  // Attach //
  ////////////
  // on attact file
  onAttactFileToDataBase(RequireQualityControlId: number, Attacts: FileList, CreateBy: string): void {
    this.serviceRequireQc.postAttactFile(RequireQualityControlId, Attacts, CreateBy)
      .subscribe(complate => console.log("Upload Complate"), error => console.error(error));
  }

  // on remove file
  onRemoveFileFromDataBase(Attachs: Array<number>): void {
    Attachs.forEach((value: number) => {
      this.serviceRequireQc.deleteAttactFile(value)
        .subscribe(complate => console.log("Delete Complate"), error => console.error(error));
    });
  }

  // on back from report
  onBack(): void {
    if (this.displayValue) {
      this.loadReport = !this.loadReport;
    }
    if (this.backToSchedule) {
      this.location.back();
    }
  }

  // on show report
  onReport(Value?: RequireQc): void {
    if (Value) {
      this.loadReport = !this.loadReport;
    }
  }
}
