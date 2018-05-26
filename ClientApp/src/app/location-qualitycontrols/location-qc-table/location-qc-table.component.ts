// Angular Core
import { Component } from "@angular/core";
// Components
import { CustomMatTableComponent } from "../../shared/custom-mat-table/custom-mat-table.component";
// Models
import { LocationQc } from "../shared/location-qc";
// Services
import { AuthService } from "../../core/auth/auth.service";
import { LocationQcService } from "../shared/location-qc.service";

@Component({
  selector: 'app-location-qc-table',
  templateUrl: './location-qc-table.component.html',
  styleUrls: ['./location-qc-table.component.scss']
})
export class LocationQcTableComponent extends CustomMatTableComponent<LocationQc, LocationQcService>{
  // Constructor
  constructor(
    service: LocationQcService,
    authService: AuthService,
  ) {
    super(service, authService);
    this.displayedColumns = ["select", "Name", "TotalGroup"];
  }
}
