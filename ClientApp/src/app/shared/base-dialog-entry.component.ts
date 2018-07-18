import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { Scroll } from "./scroll.model";
// rxjs
import { Observable, Subscription } from "rxjs";
import { BaseModel } from "./base-model.model";
import { BaseRestService } from "./base-rest.service";
// 3rd party
// import { DatatableComponent, TableColumn } from "@swimlane/ngx-datatable";

/** base-dialog component*/
export abstract class BaseDialogEntryComponent<Model extends BaseModel, Service extends BaseRestService<Model>> implements OnInit {
  getValue: Model;
  getValues: Array<Model>;
  fastSelectd: boolean = false;
  /** cutting-plan-dialog ctor */
  constructor(
    protected service: Service,
    protected dialogRef: MatDialogRef<any>
  ) { }

  // Parameter
  InfoValue: Model;
  needUpdate: boolean = true;

  /** Called by Angular after cutting-plan-dialog component initialized */
  ngOnInit(): void {
    this.onInit();
  }
  // on Init data
  abstract onInit(): void;
  // on new entity
  abstract onNewEntity(): void;

  // Selected Value
  onSelectedValue(value?: Model): void {
    // console.log("OnSelectedValue", JSON.stringify(value));
    if (value) {
      this.getValue = value;
      if (this.fastSelectd) {
        this.onSelectedClick();
      }
    }
  }

  onSelectedValues(values?: Array<Model>): void {
    if (values) {
      this.getValues = new Array;
      this.getValues = [...values];
    }
  }

  // No Click
  onCancelClick(): void {
    this.dialogRef.close();
  }

  // Update Click
  onSelectedClick(): void {
    if (this.getValue) {
      this.dialogRef.close(this.getValue);
    } else if (this.getValues) {
      this.dialogRef.close(this.getValues);
    }
  }

  // on complate or cancel entity
  onComplateOrCancelEntity(infoValue?: Model): void {
    if (infoValue) {
      if (this.needUpdate) {
        if (infoValue.CreateDate) {
          this.service.updateModelWithKey(infoValue)
            .subscribe(dbData => {
              if (dbData) {
                this.dialogRef.close(dbData);
              }
            });
        } else {
          this.service.addModel(infoValue)
            .subscribe(dbData => {
              if (dbData) {
                this.dialogRef.close(dbData);
              }
            });
        }
      } else {
        this.getValue = infoValue;
      }
      
    } else {
      this.InfoValue = undefined;
    }
  }

  // on detail view
  onEditInfo(value?: { data: Model, option: number }): void {
    if (value) {
      if (value.option === 1) {
        this.InfoValue = value.data;
      }
    }
  }
}
