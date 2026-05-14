import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import { IResult } from "../../../core/interfaces/result";
import { IDashboardData } from "../interfaces/dashboard";

@Injectable({
  providedIn: "root",
})
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/dashboard`;

  getDashboardData(): Observable<IResult<IDashboardData>> {
    return this.http.get<IResult<IDashboardData>>(this.apiUrl, {
      withCredentials: true,
    });
  }
}
