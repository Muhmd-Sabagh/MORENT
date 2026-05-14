import { Component, inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import {
  BehaviorSubject,
  catchError,
  filter,
  map,
  of,
  shareReplay,
  switchMap,
  tap,
} from "rxjs";
import { CarService } from "../../services/car.service";
import { ICarDto } from "../../interfaces/car";

@Component({
  selector: "app-car-details",
  templateUrl: "./car-details.html",
  styleUrls: ["./car-details.css"],
  standalone: false,
})
export class CarDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(true);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  private readonly errorMessageSubject = new BehaviorSubject<string>("");
  readonly errorMessage$ = this.errorMessageSubject.asObservable();

  // Extract ID from URL and fetch car details reactively
  readonly car$ = this.route.paramMap.pipe(
    map((params) => params.get("id")),
    filter((id): id is string => id !== null),
    tap(() => {
      this.isLoadingSubject.next(true);
      this.errorMessageSubject.next("");
    }),
    switchMap((id) => this.carService.getCarDetails(id)),
    map((res) => {
      this.isLoadingSubject.next(false);
      if (!res.isSuccess) {
        this.errorMessageSubject.next(
          res.message || "Failed to load car details.",
        );
        return null;
      }
      return res.dataObject;
    }),
    catchError((err) => {
      this.isLoadingSubject.next(false);
      this.errorMessageSubject.next("An unexpected error occurred.");
      return of(null);
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  // Fetch recommended cars for the bottom section
  readonly recommendedCars$ = this.carService.getPopularCars(4).pipe(
    map((res) => (res.isSuccess ? res.dataObject : [])),
    catchError(() => of([] as ICarDto[])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  onRentNow(carId: string): void {
    this.router.navigate(["/rentals/checkout", carId]);
  }

  onCarClicked(carId: string): void {
    // Navigate to the new car's detail page and scroll to top
    this.router.navigate(["/cars/details", carId]).then(() => {
      window.scrollTo(0, 0);
    });
  }
}
