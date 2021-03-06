// Angular Core
import { MatDialogRef, MatDialog, MatDialogConfig, transformMenu } from "@angular/material";
import { Injectable, ViewContainerRef } from "@angular/core";
// rxjs
import { Observable } from "rxjs/Rx";
// components
import {
  ConfirmDialog, ContextDialog,
  ErrorDialog,
  GroupmisDialogComponent,
  EmployeeDialogComponent,
  ProjectDialogComponent,
} from "../dialog.index";
// module
import { Employee } from "../../employees/shared/employee.model";
import { EmployeeGroupMis } from "../../employees/shared/employee-group-mis.model";
import { ProjectMaster } from "../../projects/shared/project-master.model";
import { observableToBeFn } from "rxjs/testing/TestScheduler";
import { MasterList } from "../../master-lists/shared/master-list.model";
import { MasterListDialogComponent } from "../master-list-dialog/master-list-dialog.component";
import { concat } from "rxjs/operator/concat";
import { retry } from "rxjs/operators";
import { LocationQc } from "../../location-qualitycontrols/shared/location-qc";
import { LocationDialogComponent } from "../location-dialog/location-dialog.component";
import { RequireQcDialogComponent } from "../require-qc-dialog/require-qc-dialog.component";
import { QualitycontrolDialogComponent } from "../qualitycontrol-dialog/qualitycontrol-dialog.component";
import { QcWelder } from "../../quality-control-welders/shared/qc-welder.model";
import { QualityControlWeldersDialogComponent } from "../quality-control-welders-dialog/quality-control-welders-dialog.component";

@Injectable()
export class DialogsService {
  // width and height > width and height in scss master-dialog
  width: string = "950px";
  height: string = "500px";

  constructor(private dialog: MatDialog) { }

  public confirm(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

    let dialogRef: MatDialogRef<ConfirmDialog>;
    let config: MatDialogConfig = new MatDialogConfig();
    config.viewContainerRef = viewContainerRef;

    dialogRef = this.dialog.open(ConfirmDialog, config);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;

    return dialogRef.afterClosed();
  }

  public context(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

    let dialogRef: MatDialogRef<ContextDialog>;
    let config: MatDialogConfig = new MatDialogConfig();
    config.viewContainerRef = viewContainerRef;

    dialogRef = this.dialog.open(ContextDialog, config);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;

    return dialogRef.afterClosed();
  }

  public error(title: string, message: string, viewContainerRef: ViewContainerRef): Observable<boolean> {

    let dialogRef: MatDialogRef<ErrorDialog>;
    let config: MatDialogConfig = new MatDialogConfig();
    config.viewContainerRef = viewContainerRef;

    dialogRef = this.dialog.open(ErrorDialog, config);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;

    return dialogRef.afterClosed();
  }
  /**
   * 
   * @param viewContainerRef
   * @param type = mode of project dialog
   */
  public dialogSelectProject(viewContainerRef: ViewContainerRef, type: number = 0): Observable<ProjectMaster> {
    let dialogRef: MatDialogRef<ProjectDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(ProjectDialogComponent, config);
    return dialogRef.afterClosed();
  }
 
  /**
   * Group Mis
   * @param viewContainerRef
   * @param type = mode 0:fastSelected
   */
  public dialogSelectGroupMis(viewContainerRef: ViewContainerRef, type: number = 0): Observable<EmployeeGroupMis> {
    let dialogRef: MatDialogRef<GroupmisDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(GroupmisDialogComponent, config);
    return dialogRef.afterClosed();
  }

  /**
   * Group Mis
   * @param viewContainerRef
   * @param type = mode 0:fastSelected
   */
  public dialogSelectGroupMises(viewContainerRef: ViewContainerRef, type: number = 1): Observable<Array<EmployeeGroupMis>> {
    let dialogRef: MatDialogRef<GroupmisDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(GroupmisDialogComponent, config);
    return dialogRef.afterClosed();
  }


  /**
   * @param viewContainerRef
   * @param type = mode 0:fastSelected
   */
  public dialogSelectEmployee(viewContainerRef: ViewContainerRef, type: number = 0): Observable<Employee> {
    let dialogRef: MatDialogRef<EmployeeDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(EmployeeDialogComponent, config);
    return dialogRef.afterClosed();
  }

  /**
 * @param viewContainerRef
 * @param type = mode 0:fastSelected
 */
  public dialogSelectEmployees(viewContainerRef: ViewContainerRef, type: number = 1): Observable<Array<Employee>> {
    let dialogRef: MatDialogRef<EmployeeDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(EmployeeDialogComponent, config);
    return dialogRef.afterClosed();
  }
  /**
   * Dialog create mark no
   * @param viewContainerRef
   * @param masterList
   */
  public dialogCreateMarkNo(viewContainerRef: ViewContainerRef, masterList: MasterList): Observable<MasterList> {
    let dialogRef: MatDialogRef<MasterListDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    //config
    config.viewContainerRef = viewContainerRef;
    config.data = masterList;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(MasterListDialogComponent, config);
    return dialogRef.afterClosed();
  }

  /**
   * 
   * @param viewContainerRef
   * @param type
   */
  public dialogSelectLocationAndCreateLocation(viewContainerRef: ViewContainerRef, type: number = 1): Observable<LocationQc> {
    let dialogRef: MatDialogRef<LocationDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = type;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(LocationDialogComponent, config);
    return dialogRef.afterClosed();
  }

  /**
 * RequireQualityControl
 * @param viewContainerRef
 * @param type
 */
  public dialogSelectRequireQualityControl(RequireQualityControlId: number, viewContainerRef: ViewContainerRef, ShowCommand: boolean = true): Observable<number> {
    let dialogRef: MatDialogRef<RequireQcDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    let data: { RequireQualityControlId: number, ShowCommand: boolean };
    data = {
      RequireQualityControlId: RequireQualityControlId,
      ShowCommand: ShowCommand
    };
    // config
    config.viewContainerRef = viewContainerRef;
    config.data = data;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(RequireQcDialogComponent, config);
    return dialogRef.afterClosed();
  }

  /**
   * QualityControl
   * @param QualityControlId
   * @param viewContainerRef
   */
  public dialogSelectQualityControl(QualityControlId: number, viewContainerRef: ViewContainerRef): Observable<number> {
    let dialogRef: MatDialogRef<QualitycontrolDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    // config
    config.viewContainerRef = viewContainerRef;
    config.data = QualityControlId;
    // config.height = this.height;
    // config.width= this.width;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(QualitycontrolDialogComponent, config);
    return dialogRef.afterClosed();
  }
  /**
   * Dialog create quailty control welder data
   * @param viewContainerRef
   * @param masterList
   */
  public dialogCreateOrUpdateWelderInfo(viewContainerRef: ViewContainerRef, qcWelder: QcWelder): Observable<QcWelder> {
    let dialogRef: MatDialogRef<QualityControlWeldersDialogComponent>;
    let config: MatDialogConfig = new MatDialogConfig();

    //config
    config.viewContainerRef = viewContainerRef;
    config.data = qcWelder;
    config.hasBackdrop = true;

    // open dialog
    dialogRef = this.dialog.open(QualityControlWeldersDialogComponent, config);
    return dialogRef.afterClosed();
  }

}
