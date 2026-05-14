import { Injectable, inject } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import { IPagedResult, IResult } from "../../../core/interfaces/result";
import { ICarDto } from "../interfaces/car";
import { ILocationDto } from "../interfaces/location";

@Injectable({
  providedIn: "root",
})
export class CarService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/cars`;

  getAvailableLocations(): Observable<IResult<ILocationDto[]>> {
    return this.http.get<IResult<ILocationDto[]>>(`${this.apiUrl}/locations`);
  }

  getPopularCars(count: number = 4): Observable<IResult<ICarDto[]>> {
    return this.http.get<IResult<ICarDto[]>>(
      `${this.apiUrl}/popular?count=${count}`,
    );
  }

  getRecommendedCars(count: number = 4): Observable<IResult<ICarDto[]>> {
    return this.http.get<IResult<ICarDto[]>>(
      `${this.apiUrl}/recommended?count=${count}`,
    );
  }

  getFilteredCars(
    pageNumber: number = 1,
    pageSize: number = 9,
    pickUpLocationId?: number,
    searchTerm?: string,
    carType?: string,
    capacity?: number,
    steeringType?: string,
    maxPrice?: number,
    pickUpDate?: string,
    dropOffDate?: string,
  ): Observable<IResult<IPagedResult<ICarDto>>> {
    let params = new HttpParams()
      .set("pageNumber", pageNumber)
      .set("pageSize", pageSize);

    if (pickUpLocationId)
      params = params.set("pickUpLocationId", pickUpLocationId);
    if (searchTerm) params = params.set("searchTerm", searchTerm);
    if (carType) params = params.set("carType", carType);
    if (capacity) params = params.set("capacity", capacity);
    if (steeringType) params = params.set("steeringType", steeringType);
    if (maxPrice) params = params.set("maxPrice", maxPrice);
    if (pickUpDate) params = params.set("pickUpDate", pickUpDate);
    if (dropOffDate) params = params.set("dropOffDate", dropOffDate);

    return this.http.get<IResult<IPagedResult<ICarDto>>>(this.apiUrl, {
      params,
    });
  }

  getCarDetails(id: string): Observable<IResult<ICarDto>> {
    return this.http.get<IResult<ICarDto>>(`${this.apiUrl}/${id}`);
  }

  getFavorites(): Observable<IResult<ICarDto[]>> {
    return this.http.get<IResult<ICarDto[]>>(`${this.apiUrl}/favorites`, {
      withCredentials: true,
    });
  }

  toggleFavorite(carId: string): Observable<IResult<boolean>> {
    return this.http.post<IResult<boolean>>(
      `${this.apiUrl}/${carId}/favorite`,
      {},
      { withCredentials: true },
    );
  }
}
