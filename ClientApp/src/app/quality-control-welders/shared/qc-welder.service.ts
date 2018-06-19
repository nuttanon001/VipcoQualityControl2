// Angular Core
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";
// Services
import { BaseRestService } from '../../shared/base-rest.service';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
// Models
import { QcWelder } from './qc-welder.model';
import { AutoComplate } from '../../dialogs/shared/auto-complate';
import { ResultAutoComplate } from '../../dialogs/shared/result-auto-complate';

@Injectable()
export class QcWelderService extends BaseRestService<QcWelder> {

  constructor(
    http: HttpClient,
    httpErrorHandler:HttpErrorHandler,
  ) {
    super(
      http,
      "api/QualityControlWelding/",
      "QualityControlWeldingService",
      "QualityControlWeldingId",
      httpErrorHandler
    );
  }

  // ===================== Quality Control Welder Auto Complate ===========================\\
  // Master Project auto complate
 getAutoComplateEdition(autoComplate:AutoComplate): Observable<Array<ResultAutoComplate>> {
    return this.http.post<Array<ResultAutoComplate>>(
      this.baseUrl + "Autocomplate/",
      JSON.stringify(autoComplate),
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json",
        })
      }).pipe(catchError(this.handleError(this.serviceName + "/Quality Control Welder autocomplate", Array<ResultAutoComplate>())));
  }
}
