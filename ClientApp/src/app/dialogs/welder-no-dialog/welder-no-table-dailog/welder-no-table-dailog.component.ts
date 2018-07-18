import { Component, OnInit } from '@angular/core';
import { WelderNoTableComponent } from '../../../welder-no/welder-no-table/welder-no-table.component';
import { WelderNoService } from '../../../welder-no/shared/welder-no.service';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-welder-no-table-dailog',
  templateUrl: '../../../welder-no/welder-no-table/welder-no-table.component.html',
  styleUrls: ['./welder-no-table-dailog.component.scss']
})
export class WelderNoTableDailogComponent extends WelderNoTableComponent {

  constructor(
    service: WelderNoService,
    serviceAuth: AuthService
  ) {
    super(service, serviceAuth);
  }
}
