import { Component, inject } from "@angular/core";
import {
  BehaviorSubject,
  catchError,
  map,
  of,
  shareReplay,
  switchMap,
  tap,
} from "rxjs";
import { DashboardService } from "../../services/dashboard.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.html",
  styleUrls: ["./dashboard.css"],
  standalone: false,
})
export class Dashboard {
  private readonly dashboardService = inject(DashboardService);

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(true);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  readonly dashboardData$ = of(void 0).pipe(
    tap(() => this.isLoadingSubject.next(true)),
    switchMap(() => this.dashboardService.getDashboardData()),
    map((res) => (res.isSuccess ? res.dataObject : null)),
    catchError(() => of(null)),
    tap(() => this.isLoadingSubject.next(false)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );
}
