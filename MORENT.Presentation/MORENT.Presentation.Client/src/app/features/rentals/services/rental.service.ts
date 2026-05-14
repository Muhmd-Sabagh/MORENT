import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import { IResult } from "../../../core/interfaces/result";
import { ICreateRentalRequest, IRentalDto } from "../interfaces/rental";
import { IPaymentMethodDto } from "../interfaces/payment-method";

@Injectable({
  providedIn: "root",
})
export class RentalService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/rentals`;

  getAvailabePaymentMethods(): Observable<IResult<IPaymentMethodDto[]>> {
    return this.http.get<IResult<IPaymentMethodDto[]>>(
      `${this.apiUrl}/payment-methods`,
      {
        withCredentials: true,
      },
    );
  }

  createRental(request: ICreateRentalRequest): Observable<IResult<string>> {
    return this.http.post<IResult<string>>(this.apiUrl, request, {
      withCredentials: true,
    });
  }

  getMyRentals(): Observable<IResult<IRentalDto[]>> {
    return this.http.get<IResult<IRentalDto[]>>(`${this.apiUrl}/my-rentals`, {
      withCredentials: true,
    });
  }

  getRentalDetails(id: string): Observable<IResult<IRentalDto>> {
    return this.http.get<IResult<IRentalDto>>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }
}
