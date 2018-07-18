import { Component, OnInit, ViewContainerRef, ViewChild } from '@angular/core';
import { BaseMasterComponent } from '../../shared/mk2/base-master-component';
import { AuthService } from '../../core/auth/auth.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { WelderProjectCommunicateService } from '../shared/welder-project-communicate.service';
import { WelderProjectService } from '../shared/welder-project.service';
import { WelderProjectTableComponent } from '../welder-project-table/welder-project-table.component';
import { WelderProjectHasEmp } from '../shared/welder-project-has-emp.model';

@Component({
  selector: 'app-welder-project-master',
  templateUrl: './welder-project-master.component.html',
  styleUrls: ['./welder-project-master.component.scss']
})
export class WelderProjectMasterComponent extends BaseMasterComponent<WelderProjectHasEmp, WelderProjectService, WelderProjectCommunicateService> {
  constructor(
    service: WelderProjectService,
    serviceCom: WelderProjectCommunicateService,
    serviceAuth: AuthService,
    serviceDia: DialogsService,
    viewConRef: ViewContainerRef
  ) { super(service, serviceCom, serviceAuth, serviceDia, viewConRef); }

  @ViewChild(WelderProjectTableComponent)
  private tableComponent: WelderProjectTableComponent;

  onReloadData(): void {
    this.tableComponent.reloadData();
  }
}
