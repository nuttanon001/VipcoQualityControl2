// angular
import { Component, ViewContainerRef, ViewChild, Input } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { LocationQc } from "../shared/location-qc";
// components
import { BaseEditComponent } from "../../shared/base-edit-component";
// services
import { AuthService } from "../../core/auth/auth.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { LocationQcService, LocationQcCommunicateService } from "../shared/location-qc.service";
import { WorkgroupHasWorkshop } from "../shared/workgroup-has-workshop.model";

@Component({
  selector: 'app-location-qc-edit',
  templateUrl: './location-qc-edit.component.html',
  styleUrls: ['./location-qc-edit.component.scss']
})
export class LocationQcEditComponent extends BaseEditComponent<LocationQc, LocationQcService> {
  constructor(
    service: LocationQcService,
    serviceCom: LocationQcCommunicateService,
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
  // Parameter
  @Input() ReadOnly: boolean = true;

  // on get data by key
  onGetDataByKey(value?: LocationQc): void {
    if (value && value.LocationQualityControlId) {
      this.service.getOneKeyNumber(value)
        .subscribe(dbData => {
          this.editValue = dbData;
        }, error => console.error(error), () => this.buildForm());
    } else {
      this.editValue = {
        LocationQualityControlId: 0,
        WorkGroupHasWorkShops: new Array
      };
      this.buildForm();
    }
  }

  // build form
  buildForm(): void {
    this.editValueForm = this.fb.group({
      LocationQualityControlId: [this.editValue.LocationQualityControlId],
      Name: [this.editValue.Name,
      [
        Validators.required,
        Validators.maxLength(50)
      ]
      ],
      Description: [this.editValue.Description],
      //BaseModel
      Creator: [this.editValue.Creator],
      CreateDate: [this.editValue.CreateDate],
      Modifyer: [this.editValue.Modifyer],
      ModifyDate: [this.editValue.ModifyDate],
      //Fk
      WorkGroupHasWorkShops: [this.editValue.WorkGroupHasWorkShops],
    });
    this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
  }

  // open dialog
  openDialog(type?: string): void {
    if (this.ReadOnly) { return; }
    if (this.editValueForm) {
      this.editValue = this.editValueForm.value;
      if (!this.editValue.Name) { return; }
    } else { return; }

    if (type) {
      if (type === "GroupOfWork") {
        this.serviceDialogs.dialogSelectGroupMises(this.viewContainerRef, 1)
          .subscribe(groups => {
            if (groups) {
              groups.forEach((group, index) => {
                if (!this.editValue.WorkGroupHasWorkShops.find(groupCode => groupCode.GroupMis === group.GroupMis)) {
                  this.editValue.WorkGroupHasWorkShops.push({
                    GroupMis: group.GroupMis,
                    GroupMisString: group.GroupDesc,
                    WorkGroupHasWorkShopId: 0,
                  });
                }
              });

              this.editValue.WorkGroupHasWorkShops = this.editValue.WorkGroupHasWorkShops.slice();
              // Update to form
              this.editValueForm.patchValue({
                WorkGroupHasWorkShops: this.editValue.WorkGroupHasWorkShops
              });
            }
          });
      }
    }
  }

  // ItemEmployee remove
  onItemRemove(anyData?: { data: WorkgroupHasWorkshop, mode: number }): void {
    //console.log("Data is", JSON.stringify(anyData));
    if (anyData) {
      if (anyData.mode === 0) {
        // Found Index
        let indexItem: number = this.editValue.WorkGroupHasWorkShops.indexOf(anyData.data);
        // Remove at Index
        this.editValue.WorkGroupHasWorkShops.splice(indexItem, 1);
        // Angular need change data for update view
        this.editValue.WorkGroupHasWorkShops = this.editValue.WorkGroupHasWorkShops.slice();
        // Update to form
        this.editValueForm.patchValue({
          WorkGroupHasWorkShops: this.editValue.WorkGroupHasWorkShops
        });
      }
    }
  }
}
