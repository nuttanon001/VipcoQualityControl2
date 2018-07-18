import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
// services
import { HttpErrorHandler } from "../../shared/http-error-handler.service"
// models
import { MasterList } from "./master-list.model";
// Base-Services
import { BaseRestService } from "../../shared/base-rest.service";
import { BaseCommunicateService } from "../../shared/base-communicate.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";
import { ResultAutoComplate } from "../../dialogs/shared/result-auto-complate";
import { AutoComplate } from "../../dialogs/shared/auto-complate";

@Injectable()
export class MasterListService extends BaseRestService<MasterList> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/MasterProjectList/",
      "MasterListService",
      "MasterProjectListId",
      httpErrorHandler
    )
  }

  // ===================== Master Project List Auto Complate ===========================\\
  // Master Project auto complate
  masterProjectListAutoComplate(Filter: string): Observable<Array<MasterList>> {
    const options = {
      params: new HttpParams().set("Filter", Filter)
    };
    return this.http.get<Array<MasterList>>(this.baseUrl + "Autocomplate/", options)
      .pipe(catchError(this.handleError(this.serviceName + "/Master list autocomplate", Array<MasterList>())));
  }
  // ===================== MasterProjectList by RequireQualityControl ===========================\\
  // action master project list
  actionRequireQualityControlHasMarkNo(RequireQualityControlId: number, SubAction: string = "GetMasterProjectListByRequireQualityControl/"): Observable<Array<MasterList>> {
    const options = { params: new HttpParams().set("key", RequireQualityControlId.toString()) };
    return this.http.get<Array<MasterList>>(this.baseUrl + SubAction, options)
      .pipe(catchError(this.handleError(this.serviceName + "/get master project list by require quality control model", Array<MasterList>())));
  }
  // ===================== Quality Control Welder Auto Complate ===========================\\
  // Master Project auto complate
  getAutoComplateEdition(autoComplate: AutoComplate): Observable<Array<ResultAutoComplate>> {
    return this.http.post<Array<ResultAutoComplate>>(
      this.baseUrl + "Autocomplate/",
      JSON.stringify(autoComplate),
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).pipe(catchError(this.handleError("Get autocomplate from api", Array<ResultAutoComplate>())));
  }
}

@Injectable()
export class MasterListCommunicateService extends BaseCommunicateService<MasterList> {
  constructor() { super(); }
}
