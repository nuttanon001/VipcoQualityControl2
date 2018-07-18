import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
// service
import { HttpErrorHandler } from "../../shared/http-error-handler.service";
// models
import { EmployeeGroupMis } from "../../employees/shared/employee-group-mis.model";
// component
import { BaseRestService } from "../../shared/base-rest.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";

@Injectable()
export class EmployeeGroupMisService extends BaseRestService<EmployeeGroupMis> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/EmployeeGroupMis/",
      "EmployeeGroupMisService",
      "GroupMIS",
      httpErrorHandler
    )
  }

  // ===================== EmployeeGroupMis ===========================\\
  // action require maintenance
  getGroupMinsByEmpCode(EmpCode: string): Observable<EmployeeGroupMis> {

    return this.http.get<EmployeeGroupMis>(this.baseUrl + "GroupMisByEmpCode/", {
      params: new HttpParams().set("EmpCode", EmpCode.toString())
    }).pipe(catchError(this.handleError("Get employee work group by empcode from api", <EmployeeGroupMis>{})));
  }
}
