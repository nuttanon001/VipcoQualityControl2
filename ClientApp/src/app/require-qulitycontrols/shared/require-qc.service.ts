import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
// services
import { HttpErrorHandler } from "../../shared/http-error-handler.service";
// models
import { RequireQc } from "./require-qc.model";
import { RequireQcChange } from "./require-qc-change";
import { OptionRequireQc } from "./option-require-qc.model";
import { AttachFile } from "../../shared/attach-file.model";
// Base-Services
import { BaseRestService } from "../../shared/base-rest.service";
import { BaseCommunicateService } from "../../shared/base-communicate.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";
import { RequireQcSchedule } from "./require-qc-schedule.model";

@Injectable()
export class RequireQualityControlService extends BaseRestService<RequireQc> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/RequireQualityControl/",
      "RequireQualityControlService",
      "RequireQualityControlId",
      httpErrorHandler
    )
  }

  // ===================== RequireQualityControlSchedule ===========\\
  /**
   * Require Quality Control Schedule
   * @param option = option for schedul require quality control
   */
  getRequireQualityControlSchedule2(option: RequireQcSchedule): Observable<any> {
    return this.http.post<any>(this.baseUrl + "RequireQualityControlSchedule/", JSON.stringify(option), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    }).pipe(catchError(
      this.handleError(this.serviceName + "/get require quality control schedule", new Array<any>())));
  }
  // ===================== GenarateFromFailRequireQualityControl ===\\
  getGenarateFromFailRequireQualityControl(parentRequireQcId: number): Observable<RequireQc> {
    return this.http.get<RequireQc>(`${this.baseUrl}GenarateFromFailRequireQualityControl/`, {
      params: new HttpParams().set("key", parentRequireQcId.toString())
    }).pipe(catchError(this.handleError(this.serviceName + "/genarate from fail require quality contol", <RequireQc>{})));
  }

  // ===================== RequireQualityControl Change ============\\
  getRequireQualityControlChange(requireQcChange: RequireQcChange): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}RequireQualityControlChange/`, JSON.stringify(requireQcChange), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    }).pipe(catchError(this.handleError(this.serviceName + "/require quality control change", <any>{})));
  }
  // ===================== RequireQualityControl Schedule ==========\\
  // get Require QualityControl Schedule
  getRequireQualityControlWaiting(option: OptionRequireQc): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}RequireQualityControlWaiting/`, JSON.stringify(option), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    }).pipe(catchError(this.handleError(this.serviceName + "/require quality control waiting", <any>{})));
  }

  // ===================== Over Ride ===============================\\
  /** add Model @param nObject */
  addModel(nObject: RequireQc): Observable<RequireQc> {
    return this.http.post<RequireQc>(this.baseUrl + "CreateV2/", JSON.stringify(nObject),
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).pipe(catchError(this.handleError(this.serviceName + "/post model", nObject)));
  }

  /** update with key number */
  updateModelWithKey(uObject: RequireQc): Observable<RequireQc> {
    return this.http.put<RequireQc>(this.baseUrl + "UpdateV2/", JSON.stringify(uObject), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      params: new HttpParams().set("key", uObject[this.keyName].toString())
    }).pipe(catchError(this.handleError(this.serviceName + "/put update model", uObject)));
  }

  // ===================== Action Require Quailty Control ==========\\
  // action require quality control
  actionRequireQualityControl(RequireQualityControlId: number, ByEmployee: string): Observable<RequireQc> {
    const options = {
      params: new HttpParams().set("key", RequireQualityControlId.toString()).set("byEmp", ByEmployee)
    };
    return this.http.get<RequireQc>(this.baseUrl + "ActionRequireQualityControl/", options)
      .pipe(catchError(this.handleError(this.serviceName + "/action require quality control model", <RequireQc>{})));
  }

  // ===================== Upload File ===============================\\
  // get file
  getAttachFile(RequireQualityControlId: number): Observable<Array<AttachFile>> {
    return this.http.get<Array<AttachFile>>(this.baseUrl + "GetAttach/",
      { params: new HttpParams().set("key", RequireQualityControlId.toString()) })
      .pipe(catchError(this.handleError(this.serviceName + "/get attach file.", Array<AttachFile>())));
  }

  // upload file
  postAttactFile(RequireQualityControlId: number, files: FileList, CreateBy: string): Observable<any> {
    let input: any = new FormData();
    for (let i: number = 0; i < files.length; i++) {
      if (files[i].size <= 5242880) {
        input.append("files", files[i]);
      }
    }
    return this.http.post<any>(`${this.baseUrl}PostAttach/`, input,
      { params: new HttpParams().set("key", RequireQualityControlId.toString()).set("CreateBy", CreateBy) })
      .pipe(catchError(this.handleError(this.serviceName + "/upload attach file", <any>{})));
  }

  // delete file
  deleteAttactFile(AttachId: number): Observable<any> {
    return this.http.delete<any>(this.baseUrl + "DeleteAttach/",
      { params: new HttpParams().set("AttachFileId", AttachId.toString()) })
      .pipe(catchError(this.handleError(this.serviceName + "/delete attach file", <any>{})));
  }

  // ===================== End Upload File ===========================\\
}

@Injectable()
export class RequireQualityControlCommunicateService extends BaseCommunicateService<RequireQc> {
  constructor() { super(); }
}
