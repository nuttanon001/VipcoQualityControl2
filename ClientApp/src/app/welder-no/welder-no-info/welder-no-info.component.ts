import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { BaseInfoComponent } from '../../shared/mk2/base-info-component';
import { WelderNo } from '../shared/welder-no.model';
import { WelderNoService } from '../shared/welder-no.service';
import { WelderNoCommunicateService } from '../shared/welder-no-communicate.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { FormBuilder, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Employee } from '../../employees/shared/employee.model';

@Component({
  selector: 'app-welder-no-info',
  templateUrl: './welder-no-info.component.html',
  styleUrls: ['./welder-no-info.component.scss']
})

export class WelderNoInfoComponent extends BaseInfoComponent<WelderNo, WelderNoService, WelderNoCommunicateService> {
  constructor(
    service: WelderNoService,
    serviceCom: WelderNoCommunicateService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder
  ) {
    super(service, serviceCom);
  }

  welderTeams: Array<string>;

  //On get data from api
  onGetDataByKey(InfoValue?: WelderNo): void {
    if (InfoValue) {
      this.service.getOneKeyNumber(InfoValue)
        .subscribe(dbData => {
          this.InfoValue = dbData;
        }, error => console.error(error), () => this.buildForm());
    } else {
      this.InfoValue = {
        WelderNoId: 0,
        RegisterDate: new Date
      };
      this.buildForm();
    }
  }
  //BulidForm
  buildForm(): void {
    if (!this.welderTeams) {
      this.welderTeams = new Array;
      this.service.getTeamWelder()
        .subscribe((teams:Array<string>) => {
          if (teams) {
            this.welderTeams = teams.slice();
          }
        });
    }

    this.InfoValueForm = this.fb.group({
      WelderNoId: [this.InfoValue.WelderNoId],
      WelderNoCode: [this.InfoValue.WelderNoCode,
        [
          Validators.required,
          Validators.maxLength(50)
        ]
      ],
      TeamWelderNo: [this.InfoValue.TeamWelderNo],
      RegisterDate: [this.InfoValue.RegisterDate],
      ExprireDate: [this.InfoValue.ExprireDate],
      Description: [this.InfoValue.Description,
        [
          Validators.maxLength(200),
        ]
      ],
      Remark: [this.InfoValue.Remark,
        [
          Validators.maxLength(200),
        ]
      ],
      EmpCode: [this.InfoValue.EmpCode],
      NameThai: [this.InfoValue.NameThai, [Validators.maxLength(250)]],
      NameEnglish: [this.InfoValue.NameEnglish, [Validators.maxLength(250)]],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
      // ViewModel
      EmployeeString: [this.InfoValue.EmployeeString],
    });
    this.InfoValueForm.valueChanges.pipe(debounceTime(250), distinctUntilChanged()).subscribe(data => this.onValueChanged(data));
  }
  //OpenDialog
  openDialog(type: string = ""): void {
    if (type) {
      if (type.indexOf("Employee") !== -1) {
        this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
          .subscribe((emp: Employee) => {
            this.InfoValueForm.patchValue({
              EmpCode: emp ? emp.EmpCode : undefined,
              EmployeeString: emp ? `${emp.NameThai}` : undefined,
            });
          });
      } else if (type.indexOf("WelderTeam") !== -1) {
        this.serviceDialogs.dialogInfoWelderTeamName(this.viewContainerRef, {WelderNoId:0})
          .subscribe((welderTeam: WelderNo) => {
            this.InfoValueForm.patchValue({
              TeamWelderNo: welderTeam ? welderTeam.TeamWelderNo : undefined,
            });
            if (welderTeam && welderTeam.TeamWelderNo) {
              if (!this.welderTeams.find(name => name === welderTeam.TeamWelderNo)) {
                this.welderTeams.push(welderTeam.TeamWelderNo);
              }
            }
          });
      }
    }
  }
}
