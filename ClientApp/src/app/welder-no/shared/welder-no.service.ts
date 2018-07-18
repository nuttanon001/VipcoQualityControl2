import { Injectable } from '@angular/core';
import { BaseRestService } from '../../shared/mk2/base-rest.service';
import { HttpClient } from '@angular/common/http';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
import { WelderNo } from './welder-no.model';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';

@Injectable()
export class WelderNoService extends BaseRestService<WelderNo> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/WelderNo/",
      "WelderNoService",
      "WelderNoId",
      httpErrorHandler
    )
  }

  /** GET Models from the server */
  getTeamWelder(): Observable<any | Array<string>> {
    return this.http.get<Array<string>>(this.baseUrl +"GetTeamWelder/")
      .pipe(catchError(this.handleError("Get models.", new Array<string>())));
  }
}
