// Angular Core
import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, AbstractControl } from '@angular/forms';
// rxjs
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
// Models
// Services
import { MasterListService } from '../../../master-lists/shared/master-list.service';
// Extends
import { BaseInfoDialogComponent } from '../../../shared/base-info-dialog-component';
import { WelderNo } from '../../../welder-no/shared/welder-no.model';


@Component({
  selector: 'app-welder-team-info-dialog',
  templateUrl: './welder-team-info-dialog.component.html',
  styleUrls: ['./welder-team-info-dialog.component.scss']
})
export class WelderTeamInfoDialogComponent extends BaseInfoDialogComponent<WelderNo> {

  constructor(
    private fb: FormBuilder,
  ) { super() }
  // methods
  buildForm(): void {
    this.InfoValueForm = this.fb.group({
      WelderNoId: [this.InfoValue.WelderNoId],
      WelderNoCode: [this.InfoValue.WelderNoCode],
      TeamWelderNo: [this.InfoValue.TeamWelderNo,
        [
          Validators.required,
          Validators.maxLength(50)
        ],
      ],
      RegisterDate: [this.InfoValue.RegisterDate],
      ExprireDate: [this.InfoValue.ExprireDate],
      Description: [this.InfoValue.Description],
      Remark: [this.InfoValue.Remark],
      EmpCode: [this.InfoValue.EmpCode],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
      // ViewModel
      EmployeeString: [this.InfoValue.EmployeeString],
    });

    this.InfoValueForm.valueChanges.pipe(debounceTime(250), distinctUntilChanged()).subscribe(data => {
      if (!this.InfoValueForm) { return; }
      if (this.InfoValueForm.valid) {
        this.InfoValue = this.InfoValueForm.value;
        this.SubmitOrCancel.emit({ data: this.InfoValue, force: false });
      }
    });
  }
}
