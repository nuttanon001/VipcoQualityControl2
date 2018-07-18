import { Component, OnInit } from '@angular/core';
import { BaseTableComponent } from '../../shared/mk2/base-table.component';
import { AuthService } from '../../core/auth/auth.service';
import { WelderProjectService } from '../shared/welder-project.service';
import { WelderProjectHasEmp } from '../shared/welder-project-has-emp.model';

@Component({
  selector: 'app-welder-project-table',
  templateUrl: './welder-project-table.component.html',
  styleUrls: ['./welder-project-table.component.scss']
})
export class WelderProjectTableComponent extends BaseTableComponent<WelderProjectHasEmp, WelderProjectService> {

  constructor(service: WelderProjectService, serviceAuth: AuthService) {
    super(service, serviceAuth);
    this.displayedColumns = ["select", "ProjectCodeMasterString", "TotalWelder"];
  }
}
