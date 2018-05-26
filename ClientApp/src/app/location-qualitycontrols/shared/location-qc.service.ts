import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
// services
import { HttpErrorHandler } from "../../shared/http-error-handler.service"
// models
import { LocationQc } from "./location-qc";
// Base-Services
import { BaseRestService } from "../../shared/base-rest.service";
import { BaseCommunicateService } from "../../shared/base-communicate.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";

@Injectable()
export class LocationQcService extends BaseRestService<LocationQc> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/LocationQualityControl/",
      "LocationQcService",
      "LocationQualityControlId",
      httpErrorHandler
    )
  }

  // CheckGroupMis
  /** get one with key number */
  getCheckGroupMis(value: string): Observable<{ Has: boolean }> {
    // Add safe, URL encoded search parameter if there is a search term
    const options = value ? { params: new HttpParams().set("key", value) } : {};
    return this.http.get<{ Has: boolean }>(this.baseUrl + "CheckGroupMis/", options)
      .pipe(catchError(this.handleError(this.serviceName + "/check group mis model", <{ Has: boolean }>{})));
  }
}

@Injectable()
export class LocationQcCommunicateService extends BaseCommunicateService<LocationQc> {
  constructor() { super(); }
}
