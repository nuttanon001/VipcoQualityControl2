import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { BaseInfoComponent } from '../../shared/mk2/base-info-component';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { FormBuilder, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Employee } from '../../employees/shared/employee.model';
import { WelderProject } from '../shared/welder-project.model';
import { WelderProjectService } from '../shared/welder-project.service';
import { WelderProjectCommunicateService } from '../shared/welder-project-communicate.service';
import { ProjectMaster } from '../../projects/shared/project-master.model';
import { ProjectMasterService } from '../../projects/shared/project-master.service';
import { WelderProjectHasEmp } from '../shared/welder-project-has-emp.model';
import { WelderNo } from '../shared/welder-no.model';

@Component({
  selector: 'app-welder-project-info',
  templateUrl: './welder-project-info.component.html',
  styleUrls: ['./welder-project-info.component.scss']
})
export class WelderProjectInfoComponent extends BaseInfoComponent<WelderProjectHasEmp, WelderProjectService, WelderProjectCommunicateService> {
  constructor(
    service: WelderProjectService,
    serviceCom: WelderProjectCommunicateService,
    private serviceProject: ProjectMasterService,
    private serviceDialogs: DialogsService,
    private viewContainerRef: ViewContainerRef,
    private fb: FormBuilder
  ) {
    super(service, serviceCom);
  }

  //On get data from api
  onGetDataByKey(InfoValue?: WelderProjectHasEmp): void {
    if (InfoValue) {
      this.service.getOneKeyNumber(InfoValue)
        .subscribe(dbData => {
          this.InfoValue = Object.assign({}, dbData);
          console.log(JSON.stringify(this.InfoValue));
        }, error => console.error(error), () => this.buildForm());
    } else {
      this.InfoValue = {
        ProjectCodeMasterId: 0,
        WelderProjects: new Array
      };
      this.buildForm();
    }
  }
  //BulidForm
  buildForm(): void {

    this.InfoValueForm = this.fb.group({
      ProjectCodeMasterId: [this.InfoValue.ProjectCodeMasterId],
      ProjectCodeMasterString: [this.InfoValue.ProjectCodeMasterString,
        [
          Validators.required,
        ]
      ],
      WelderProjects: [this.InfoValue.WelderProjects],
      TotalWelder: [this.InfoValue.TotalWelder],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
    });
    this.InfoValueForm.valueChanges.pipe(debounceTime(250), distinctUntilChanged()).subscribe(data => this.onValueChanged(data));
  }
  //OpenDialog
  openDialog(type: string = ""): void {
    if (type) {
      if (type.indexOf("ProjectMaster") !== -1) {
        this.serviceDialogs.dialogSelectProject(this.viewContainerRef,1)
          .subscribe((result: ProjectMaster) => {
            this.InfoValueForm.patchValue({
              ProjectCodeMasterId: result ? result.ProjectCodeMasterId : undefined,
              ProjectCodeMasterString: result ? `${result.ProjectCode}\ ${result.ProjectName}` : undefined,
            });
          });
      } 
    }
  }
  // Parameter
  indexItem: number = -1;
  // On MachineOperator
  OnDetailSelect(Item: { data?: WelderProject, option: number }) {
    if (Item) {
      if (!Item.data) {
        this.indexItem = -1;
      } else {
        this.indexItem = this.InfoValue.WelderProjects.indexOf(Item.data);
      }

      if (Item.option === 1) {
        this.serviceDialogs.dialogSelectWelderNo(this.viewContainerRef,2)
          .subscribe((result:WelderNo[]) => {
            if (result) {
              result.forEach((item, index) => {
                if (!this.InfoValue.WelderProjects.find(itemPro => itemPro.WelderNoId === item.WelderNoId)) {
                  this.InfoValue.WelderProjects.push({
                    WelderHasProjectId: 0,
                    Description: "",
                    WelderNoId: item.WelderNoId,
                    EmployeeString: item.EmployeeString,
                  });
                }
              });
              this.InfoValue.WelderProjects = this.InfoValue.WelderProjects.slice();
              // Update to form
              this.InfoValueForm.patchValue({
                WelderProjects: this.InfoValue.WelderProjects
              });
            }
          });
      }
      else if (Item.option === 0) // Remove
      {
        this.InfoValue.WelderProjects.splice(this.indexItem, 1);
        this.InfoValue.WelderProjects = this.InfoValue.WelderProjects.slice();
        // Update to form
        this.InfoValueForm.patchValue({
          WelderProjects: this.InfoValue.WelderProjects
        });
      }
    }
  }
}
