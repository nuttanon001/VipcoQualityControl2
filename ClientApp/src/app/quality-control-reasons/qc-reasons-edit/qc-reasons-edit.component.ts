// angular
import { Component, ViewContainerRef, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// models
import { QcReasons } from "../shared/qc-reasons.model";
// components
import { BaseEditComponent } from "../../shared/base-edit-component";
// services
import { AuthService } from "../../core/auth/auth.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { ShareService } from "../../shared/share.service";
import { QcReasonsService, QcReasonsCommunicateService } from "../shared/qc-reasons.service";

@Component({
  selector: 'app-qc-reasons-edit',
  templateUrl: '../../shared/base-edit-component.html',
  styleUrls: ['./qc-reasons-edit.component.scss']
})
export class QcReasonsEditComponent extends BaseEditComponent<QcReasons, QcReasonsService> {
  constructor(
    service: QcReasonsService,
    serviceCom: QcReasonsCommunicateService,
    private serviceAuth: AuthService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder
  ) {
    super(
      service,
      serviceCom,
    );
    this.titelLabel = "Quality-Control reason form edit"
  }
  // Parameter

  // on get data by key
  onGetDataByKey(value?: QcReasons): void {
    if (value) {
      this.service.getOneKeyNumber(value)
        .subscribe(dbData => {
          this.editValue = dbData;
        }, error => console.error(error), () => this.buildForm());
    } else {
      this.editValue = {
        QualityControlReasonId: 0,
      };
      this.buildForm();
    }
  }

  // build form
  buildForm(): void {
    this.editValueForm = this.fb.group({
      InspectionPointId: [this.editValue.QualityControlReasonId],
      Name: [this.editValue.Name,
      [
        Validators.required,
        Validators.maxLength(50)
      ]
      ],
      Description: [this.editValue.Description,
      [
        Validators.maxLength(200),
      ]
      ],
      Remark: [this.editValue.Remark,
      [
        Validators.maxLength(200)
      ]
      ],
      Creator: [this.editValue.Creator],
      CreateDate: [this.editValue.CreateDate],
      Modifyer: [this.editValue.Modifyer],
      ModifyDate: [this.editValue.ModifyDate],
    });

    this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
  }
}
