import { Component, inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject, catchError, map, of, shareReplay, tap } from "rxjs";
import { CarService } from "../../cars/services/car.service";
import { ICarDto } from "../../cars/interfaces/car";

@Component({
  selector: "app-home",
  templateUrl: "./home.html",
  styleUrls: ["./home.css"],
  standalone: false,
})
export class Home {
  private readonly carService = inject(CarService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  private readonly successMessageSubject = new BehaviorSubject<string>("");
  readonly successMessage$ = this.successMessageSubject.asObservable();

  readonly checkoutSuccess$ = this.route.queryParamMap.pipe(
    map((params) => params.get("rentalSuccess")),
    tap((flag) => {
      if (flag === "1") {
        this.successMessageSubject.next("Rental created successfully.");

        setTimeout(() => {
          this.successMessageSubject.next("");
        }, 3000);

        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { rentalSuccess: null },
          queryParamsHandling: "merge",
          replaceUrl: true,
        });
      }
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  // Fetch exactly 4 popular cars
  readonly popularCars$ = this.carService.getPopularCars(4).pipe(
    map((response) => (response.isSuccess ? response.dataObject : [])),
    catchError(() => of([] as ICarDto[])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  // Fetch exactly 4 recommended cars (using getFilteredCars page 1, size 4)
  readonly recommendedCars$ = this.carService.getFilteredCars(1, 4).pipe(
    map((response) =>
      response.isSuccess && response.dataObject
        ? response.dataObject.items
        : [],
    ),
    catchError(() => of([] as ICarDto[])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  onRentCar(carId: string): void {
    this.router.navigate(["/cars/details", carId]);
  }
}
