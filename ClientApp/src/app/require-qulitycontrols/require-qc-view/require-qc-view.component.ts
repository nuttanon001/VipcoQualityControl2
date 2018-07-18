// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { RequireQc } from "../shared/require-qc.model";
import { AttachFile } from "../../shared/attach-file.model";
// components
import { BaseViewComponent } from "../../shared/base-view-component";
// services
import { RequireQualityControlService } from "../shared/require-qc.service";
import { MasterList } from "../../master-lists/shared/master-list.model";
import { MasterListService } from "../../master-lists/shared/master-list.service";
import { RequireMoreWorkactivityService } from "../shared/require-more-workactivity.service";
import { WorkActivity } from "../../work-activities/shared/work-activity.model";

@Component({
  selector: 'app-require-qc-view',
  templateUrl: './require-qc-view.component.html',
  styleUrls: ['./require-qc-view.component.scss']
})
export class RequireQcViewComponent extends BaseViewComponent<RequireQc> {
  constructor(
    private service: RequireQualityControlService,
    private serviceMasterList: MasterListService,
    private serviceMoreWorkActivity:RequireMoreWorkactivityService,
  ) {
    super();
  }
  //Parameter
  attachFiles: Array<AttachFile>;
  masterLists: Array<MasterList>;
  moreWorkActivities: string;
  noMasterList: boolean = false;
  @Input() forFail: boolean = false;
  // load more data
  onLoadMoreData(value: RequireQc) {
    this.attachFiles = new Array;
    this.moreWorkActivities = "";
    this.masterLists = new Array;
    if (value) {
      this.service.getAttachFile(value.RequireQualityControlId)
        .subscribe(dbAttach => this.attachFiles = dbAttach.slice());

      this.serviceMoreWorkActivity.getByMasterId(value.RequireQualityControlId)
        .subscribe(dbMoreWorkActivity => {
          dbMoreWorkActivity.forEach((item, index) => {
            this.moreWorkActivities += `${index + 1}.${item.WorkActivityName}`;
          });
        });

      if (!this.noMasterList) {
        this.serviceMasterList.actionRequireQualityControlHasMarkNo(value.RequireQualityControlId,
          this.forFail ? "GetMasterProjectListByRequireQualityControlForFail" : "GetMasterProjectListByRequireQualityControl" )
          .subscribe(dbMaster => {
            this.displayValue.MasterLists = new Array;

            dbMaster.forEach((item, index) => {
              this.displayValue.MasterLists.push({
                CreateDate: item.CreateDate,
                Creator: item.Creator,
                MarkNo: item.MarkNo,
                Name: item.Name,
                UnitNo: item.UnitNo,
                DrawingNo: item.DrawingNo,
                GradeMaterial1: item.GradeMaterial1,
                GradeMaterial2: item.GradeMaterial2,
                JointNumber: item.JointNumber,
                Thickness: item.Thickness,
                TypeMaterial1: item.TypeMaterial1,
                TypeMaterial2: item.TypeMaterial2,
                Box: item.Box,
                Quantity: item.Quantity,
                MasterProjectListId: item.MasterProjectListId
              });
            });
          });
      }
    }
  }
}

