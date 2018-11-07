import { Injectable } from '@angular/core';
import { BaseRestService } from '../../shared/mk2/base-rest.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';
import { Observable } from 'rxjs/Observable';
import { RequireQcHasMasterList } from '../../require-qulitycontrols/shared/require-qc-has-master-list.model';
import { catchError } from 'rxjs/operators';

@Injectable()
export class RequireQcWelderService extends BaseRestService<RequireQc> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/RequireHasWelder/",
      "RequireQcWelderService",
      "RequireHasWelderId",
      httpErrorHandler
    )
  }
  // ===================== Welder Control Report ==================== \\
  getWelderReport(RequireQcId: number): Observable<any> {
    return this.http.get<any>(this.baseUrl + "WelderReport/", {
      params: new HttpParams().set("key", RequireQcId.toString())
    }).pipe(catchError(this.handleError("Get require qc welder entity report from api", <any>{})));
  }
  // ===================== GenarateFromFailRequireQualityControl ===\\
  getGenarateFromFailRequireQualityControl(parentRequireQcId: number): Observable<RequireQc> {
    return this.http.get<RequireQc>(`${this.baseUrl}GenarateFromFailRequireQualityControl/`, {
      params: new HttpParams().set("key", parentRequireQcId.toString())
    }).pipe(catchError(this.handleError("Genarate require qc from fail require", <RequireQc>{})));
  }

  /** add Model @param nObject */
  addModel(nObject: RequireQc): Observable<RequireQc> {
    // console.log(JSON.stringify(nObject));
    return this.http.post<RequireQc>(this.baseUrl + "CreateV2/", JSON.stringify(nObject),
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).pipe(catchError(this.handleError("Insert entity to api", nObject)));
  }

  /** update with key number */
  updateModelWithKey(uObject: RequireQc): Observable<RequireQc> {
    return this.http.put<RequireQc>(this.baseUrl + "UpdateV2/", JSON.stringify(uObject), {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      params: new HttpParams().set("key", uObject["RequireQualityControlId"].toString())
    }).pipe(catchError(this.handleError("Update entity to api", uObject)));
  }
  /** get by master id */
  getByMasterId(masterId: number, subAction: string = "GetByMaster/"): Observable<any|Array<RequireQcHasMasterList>> {
    // Add safe, URL encoded search parameter if there is a search term
    const options = masterId ? { params: new HttpParams().set('key', masterId.toString()) } : {};

    let url: string = this.baseUrl + subAction;
    return this.http.get<Array<RequireQcHasMasterList>>(url, options)
      .pipe(catchError(this.handleError("Get entities with master key from api", new Array<RequireQcHasMasterList>())));
  }
}
