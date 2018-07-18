import { Injectable } from '@angular/core';
import { Dashboard } from './dashboard.model';
import { BaseRestService } from '../../shared/mk2/base-rest.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';
import { DashboardOption } from './dashboard-option.model';

@Injectable()
export class DashboardService extends BaseRestService<Dashboard> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/Home/",
      "DashboardService",
      "DashBoardDailyId",
      httpErrorHandler
    )
  }

  /** GET Models from the server */
  getDashBoardDaily(option:DashboardOption): Observable<any | Dashboard> {
    return this.http.post<Dashboard>(this.baseUrl + "GetDashBoardDaily/", JSON.stringify(option), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    }).pipe(catchError(
      this.handleError("Get dash board daily", <Dashboard>{})));
  }
}
