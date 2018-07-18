import { Injectable } from '@angular/core';
import { BaseRestService } from '../../shared/mk2/base-rest.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';
import { WelderProject } from './welder-project.model';
import { WelderProjectHasEmp } from './welder-project-has-emp.model';

@Injectable()
export class WelderProjectService extends BaseRestService<WelderProject> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/WelderHasProject/",
      "WelderProjectService",
      "WelderHasProjectId",
      httpErrorHandler
    )
  }

  /** get one with key number */
  getOneKeyNumber(value: WelderProjectHasEmp): Observable<any|WelderProjectHasEmp> {
    // Add safe, URL encoded search parameter if there is a search term
    const options = value ? { params: new HttpParams().set("key", value.ProjectCodeMasterId.toString()) } : {};
    return this.http.get<WelderProjectHasEmp>(this.baseUrl + "GetKeyNumber/", options)
      .pipe(catchError(this.handleError("Get model with key", <WelderProjectHasEmp>{})));
  }
}
