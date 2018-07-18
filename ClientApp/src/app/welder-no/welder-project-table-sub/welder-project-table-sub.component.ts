import { Component, OnInit } from '@angular/core';
import { BaseTableFontData } from '../../shared/mk2/base-table-fontdata.component';
import { WelderProject } from '../shared/welder-project.model';

@Component({
  selector: 'app-welder-project-table-sub',
  templateUrl: './welder-project-table-sub.component.html',
  styleUrls: ['./welder-project-table-sub.component.scss']
})
export class WelderProjectTableSubComponent extends BaseTableFontData<WelderProject> {
  constructor() {
    super();
    this.displayedColumns = ["EmployeeString", "Command"];
  }
}
