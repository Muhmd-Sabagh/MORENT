import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";
import {
  BehaviorSubject,
  catchError,
  map,
  of,
  scan,
  shareReplay,
  switchMap,
  tap,
} from "rxjs";
import { CarService } from "../../services/car.service";
import { ICarDto } from "../../interfaces/car";
import { IPagedResult } from "../../../../core/interfaces/result";
import { ILocationDto } from "../../interfaces/location";

@Component({
  selector: "app-catalog",
  templateUrl: "./catalog.html",
  styleUrls: ["./catalog.css"],
  standalone: false,
})
export class Catalog implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(false);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  private currentPage = 1;
  private readonly pageSize = 9;
  private readonly fetchTrigger$ = new BehaviorSubject<{ isReset: boolean }>({
    isReset: true,
  });

  // Extended form to include top-bar controls
  readonly filterForm: FormGroup = this.fb.group({
    carType: [""],
    capacity: [null],
    maxPrice: [100],
    pickUpLocationId: [""],
    pickUpDate: [""],
    pickUpTime: [""],
    dropOffLocationId: [""],
    dropOffDate: [""],
    dropOffTime: [""],
  });

  readonly locations$ = this.carService.getAvailableLocations().pipe(
    map((res) => (res.isSuccess && res.dataObject ? res.dataObject : [])),
    catchError(() => of([] as ILocationDto[])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  readonly catalogState$ = this.fetchTrigger$.pipe(
    tap(() => this.isLoadingSubject.next(true)),
    switchMap(({ isReset }) => {
      const filters = this.filterForm.value;

      // Combine Date + Time into ISO string for the backend (if Date is selected)
      let pDate: string | undefined;
      if (filters.pickUpDate) {
        pDate = new Date(
          `${filters.pickUpDate}T${filters.pickUpTime || "00:00"}`,
        ).toISOString();
      }

      let dDate: string | undefined;
      if (filters.dropOffDate) {
        dDate = new Date(
          `${filters.dropOffDate}T${filters.dropOffTime || "00:00"}`,
        ).toISOString();
      }

      return this.carService
        .getFilteredCars(
          this.currentPage,
          this.pageSize,
          filters.pickUpLocationId
            ? Number(filters.pickUpLocationId)
            : undefined,
          undefined, // searchTerm
          filters.carType || undefined,
          filters.capacity ? Number(filters.capacity) : undefined,
          undefined, // steeringType
          filters.maxPrice ? Number(filters.maxPrice) : undefined,
          pDate,
          dDate,
        )
        .pipe(
          map((res) => ({
            isReset,
            data:
              res.isSuccess && res.dataObject
                ? res.dataObject
                : ({
                    items: [],
                    totalCount: 0,
                    pageNumber: 0,
                    pageSize: 0,
                    totalPages: 0,
                  } as IPagedResult<ICarDto>),
          })),
        );
    }),
    scan(
      (acc, curr) => {
        if (curr.isReset) {
          return { items: curr.data.items, totalCount: curr.data.totalCount };
        }
        return {
          items: [...acc.items, ...curr.data.items],
          totalCount: curr.data.totalCount,
        };
      },
      { items: [] as ICarDto[], totalCount: 0 },
    ),
    tap(() => this.isLoadingSubject.next(false)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  ngOnInit(): void {
    // Top bar and sidebar both trigger native zoneless updates automatically!
    this.filterForm.valueChanges.subscribe(() => {
      this.currentPage = 1;
      this.fetchTrigger$.next({ isReset: true });
    });
  }

  onShowMore(): void {
    this.currentPage++;
    this.fetchTrigger$.next({ isReset: false });
  }

  onRentCar(carId: string): void {
    this.router.navigate(["/cars/details", carId]);
  }
}
