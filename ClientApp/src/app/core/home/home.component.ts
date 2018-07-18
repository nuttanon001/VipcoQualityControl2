import { Component,OnInit } from "@angular/core";
// Model
import { User } from "../../users/shared/user.model";
import { DashboardService } from "../../dashboards/shared/dashboard.service";
import { Dashboard } from "../../dashboards/shared/dashboard.model";
// Service
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointState, BreakpointObserver } from '@angular/cdk/layout';
@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"]
})
export class HomeComponent implements OnInit {
  cards = [
    { title: "Top 3 workshop require quality control", cols: 1, rows: 3 ,test:1},
    { title: 'Total require', cols: 1, rows: 1, id: 2},
    { title: 'Total quality control approved', cols: 1, rows: 1, test: 3},
    { title: 'Total quality control failur', cols: 1, rows: 1, test: 4}
  ];

  constructor(
    private service: DashboardService,
    private breakpointObserver: BreakpointObserver
  ) { }

  /**
   * Parameter
   */
  dashBoard: Dashboard;

  /**
   * On angular core Init
   */
  ngOnInit(): void {
    // console.log(JSON.stringify(this.cards));

    //this.service.getDashBoardDaily({ PickDate: new Date() })
    //  .subscribe(DashBoardData => {
    //    if (DashBoardData) {
    //      this.dashBoard = DashBoardData;
    //    }
    //  });
  }

  onOpenNewLink(): void {
    let link: string = "files/VipcoQualityControlDoc.pdf";
    if (link) {
      window.open(link, "_blank");
    }
  }
}
