import { Component, Input, Output, EventEmitter, inject } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";
import { ICarDto } from "../../../features/cars/interfaces/car";
import { CarService } from "../../../features/cars/services/car.service";
import { AuthService } from "../../../core/services/auth.service";

@Component({
  selector: "app-car-card",
  templateUrl: "./car-card.html",
  styleUrls: ["./car-card.css"],
  standalone: false,
})
export class CarCard {
  @Input() car!: ICarDto;

  @Input() set isFavorite(val: boolean) {
    this.isFavoriteSubject.next(val);
  }

  @Output() rentClicked = new EventEmitter<string>();

  private readonly carService = inject(CarService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Zoneless state for the Heart Icon
  private readonly isFavoriteSubject = new BehaviorSubject<boolean>(false);
  readonly isFavorite$ = this.isFavoriteSubject.asObservable();

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(false);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  onCardClick(): void {
    if (this.car && this.car.id) {
      this.router.navigate(["/cars/details", this.car.id]);
    }
  }

  onRentClick(): void {
    if (this.car && this.car.id) {
      this.rentClicked.emit(this.car.id);
    }
  }

  onToggleFavorite(event: Event): void {
    event.stopPropagation();

    // 1. Must be logged in to favorite a car
    if (!this.authService.currentUserValue) {
      this.router.navigate(["/auth/login"]);
      return;
    }

    if (this.isLoadingSubject.value) return;

    const currentState = this.isFavoriteSubject.value;

    // 2. Optimistic Update
    this.isFavoriteSubject.next(!currentState);
    this.isLoadingSubject.next(true);

    // 3. API Call
    this.carService.toggleFavorite(this.car.id).subscribe({
      next: (res) => {
        this.isLoadingSubject.next(false);
        if (res.isSuccess) {
          // Backend returns 'true' if added, 'false' if removed. Sync it.
          this.isFavoriteSubject.next(res.dataObject);
        } else {
          this.isFavoriteSubject.next(currentState);
        }
      },
      error: () => {
        this.isLoadingSubject.next(false);
        this.isFavoriteSubject.next(currentState);
      },
    });
  }
}
