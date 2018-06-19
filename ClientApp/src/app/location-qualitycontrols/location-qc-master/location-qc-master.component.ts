import { Component, ViewContainerRef, ViewChild } from "@angular/core";
// components
import { BaseMasterComponent } from "../../shared/base-master-component";
// models
import { LocationQc } from "../shared/location-qc";
// services
import { AuthService } from "../../core/auth/auth.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { LocationQcService, LocationQcCommunicateService } from "../shared/location-qc.service";
// timezone
import * as moment from "moment-timezone";
import { LocationQcTableComponent } from "../location-qc-table/location-qc-table.component";

@Component({
  selector: 'app-location-qc-master',
  templateUrl: './location-qc-master.component.html',
  styleUrls: ['./location-qc-master.component.scss']
})
export class LocationQcMasterComponent extends BaseMasterComponent<LocationQc, LocationQcService> {

  /** require-painting-master ctor */
  constructor(
    service: LocationQcService,
    serviceCom: LocationQcCommunicateService,
    authService: AuthService,
    dialogsService: DialogsService,
    viewContainerRef: ViewContainerRef,
  ) {
    super(
      service,
      serviceCom,
      authService,
      dialogsService,
      viewContainerRef
    );

    this.ReadOnly = true;
  }

  //Parameter
  ReadOnly: boolean;
  @ViewChild(LocationQcTableComponent)
  private tableComponent: LocationQcTableComponent;

  // on change time zone befor update to webapi
  changeTimezone(value: LocationQc): LocationQc {
    let zone: string = "Asia/Bangkok";
    if (value !== null) {
      if (value.CreateDate !== null) {
        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
      }
      if (value.ModifyDate !== null) {
        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
      }
    }
    return value;
  }

  // onReload
  onReloadData(): void {
    this.tableComponent.reloadData();
  }

  //////////////
  // OverRide //
  //////////////
  // on detail view
  // abstract onDetailView(value: any): void;
  onDetailView(value?: LocationQc): void {
    // debug here
    // console.log(JSON.stringify(value));

    if (this.ShowEdit) {
      return;
    }
    if (value) {
      this.ReadOnly = true;
      // console.log(value);
      this.service.getOneKeyNumber(value)
        .subscribe(dbData => {
          // console.log(JSON.stringify(dbData));
          this.displayValue = dbData;
          setTimeout(() => this.comService.toChildEdit(this.displayValue), 1000);
        }, error => this.displayValue = undefined);
    } else {
      this.displayValue = undefined;
    }
  }

  // on detail edit
  onDetailEdit(editValue?: LocationQc): void {
    this.ReadOnly = false;
    if (!editValue) {
      editValue = {
        LocationQualityControlId: 0
      };
    }
    super.onDetailEdit(editValue);
  }
 
}
