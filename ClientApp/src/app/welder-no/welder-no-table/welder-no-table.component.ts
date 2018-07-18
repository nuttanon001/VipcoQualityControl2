import { Component, OnInit } from '@angular/core';
import { BaseTableComponent } from '../../shared/mk2/base-table.component';
import { WelderNo } from '../shared/welder-no.model';
import { WelderNoService } from '../shared/welder-no.service';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-welder-no-table',
  templateUrl: './welder-no-table.component.html',
  styleUrls: ['./welder-no-table.component.scss']
})
export class WelderNoTableComponent extends BaseTableComponent<WelderNo, WelderNoService> {

  constructor(service: WelderNoService, serviceAuth: AuthService) {
    super(service, serviceAuth);
    this.displayedColumns = ["select", "WelderNoCode", "EmpCode", "TeamWelderNo", "NameThai"];
  }
}
