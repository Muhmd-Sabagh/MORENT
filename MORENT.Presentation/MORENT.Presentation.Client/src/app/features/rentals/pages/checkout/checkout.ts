import { Component, inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import {
  BehaviorSubject,
  catchError,
  filter,
  finalize,
  map,
  of,
  shareReplay,
  startWith,
  switchMap,
  tap,
} from "rxjs";
import { CarService } from "../../../cars/services/car.service";
import { RentalService } from "../../services/rental.service";
import { ILocationDto } from "../../../cars/interfaces/location";
import { IPaymentMethodDto } from "../../interfaces/payment-method";

@Component({
  selector: "app-checkout",
  templateUrl: "./checkout.html",
  styleUrls: ["./checkout.css"],
  standalone: false,
})
export class Checkout {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly carService = inject(CarService);
  private readonly rentalService = inject(RentalService);

  private paymentMethodsSnapshot: IPaymentMethodDto[] = [];

  // State Subjects
  private readonly isLoadingSubject = new BehaviorSubject<boolean>(false);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  private readonly errorMessageSubject = new BehaviorSubject<string>("");
  readonly errorMessage$ = this.errorMessageSubject.asObservable();

  // Reactive Route & Data Fetching
  private readonly carId$ = this.route.paramMap.pipe(
    map((params) => params.get("id")),
    filter((id): id is string => id !== null),
  );

  readonly carDetails$ = this.carId$.pipe(
    switchMap((id) => this.carService.getCarDetails(id)),
    map((res) => (res.isSuccess ? res.dataObject : null)),
    catchError(() => of(null)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  readonly locations$ = this.carService.getAvailableLocations().pipe(
    map((res) => (res.isSuccess && res.dataObject ? res.dataObject : [])),
    catchError(() => of([] as ILocationDto[])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  readonly paymentMethods$ = this.rentalService
    .getAvailabePaymentMethods()
    .pipe(
      map((res) => (res.isSuccess && res.dataObject ? res.dataObject : [])),
      tap((methods) => {
        this.paymentMethodsSnapshot = methods;

        const paymentControl = this.checkoutForm.get("paymentMethodId");
        if (!paymentControl) return;

        const currentValue = paymentControl.value;
        if (
          (currentValue === null || currentValue === undefined) &&
          methods.length > 0
        ) {
          paymentControl.setValue(methods[0].id);
        }
      }),
      catchError(() => of([] as IPaymentMethodDto[])),
      shareReplay({ bufferSize: 1, refCount: true }),
    );

  // Form Setup
  readonly checkoutForm: FormGroup = this.fb.group({
    billingInfo: this.fb.group({
      name: ["", Validators.required],
      phoneNumber: ["", Validators.required],
      address: ["", Validators.required],
      townCity: ["", Validators.required],
    }),
    rentalInfo: this.fb.group({
      pickUpLocationId: [null, Validators.required],
      pickUpDate: ["", Validators.required],
      dropOffLocationId: [null, Validators.required],
      dropOffDate: ["", Validators.required],
    }),
    paymentMethodId: [null, Validators.required],
    paymentDetails: this.fb.group({
      creditCard: this.fb.group({
        cardNumber: [""],
        expirationDate: [""],
        cardHolder: [""],
        cvc: [""],
      }),
      paypal: this.fb.group({
        email: [""],
      }),
      bitcoin: this.fb.group({
        walletAddress: [""],
      }),
    }),
    confirmation: this.fb.group({
      marketingConsent: [false],
      termsConsent: [false, Validators.requiredTrue],
    }),
  });

  private readonly paymentMethodIdControl =
    this.checkoutForm.get("paymentMethodId");

  readonly selectedPaymentMethodId$ = (this.paymentMethodIdControl
    ? this.paymentMethodIdControl.valueChanges
    : of(null)
  ).pipe(
    startWith(this.paymentMethodIdControl?.value ?? null),
    tap((id) => this.applyPaymentDetailsValidators(id)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  paymentMethodKey(
    name: string | undefined,
  ): "credit-card" | "paypal" | "bitcoin" | "unknown" {
    const normalized = (name ?? "").replace(/\s+/g, "").toLowerCase();
    if (normalized.includes("credit") || normalized.includes("card"))
      return "credit-card";
    if (normalized.includes("paypal")) return "paypal";
    if (normalized.includes("bitcoin") || normalized.includes("btc"))
      return "bitcoin";
    return "unknown";
  }

  private applyPaymentDetailsValidators(paymentMethodId: unknown): void {
    const id =
      typeof paymentMethodId === "number"
        ? paymentMethodId
        : Number(paymentMethodId);
    if (!Number.isFinite(id)) {
      this.clearPaymentValidators();
      return;
    }

    const selected = this.paymentMethodsSnapshot.find((m) => m.id === id);
    const key = this.paymentMethodKey(selected?.name);

    this.clearPaymentValidators();
    this.resetNonSelectedPaymentGroups(key);

    if (key === "credit-card") {
      this.setValidators("paymentDetails.creditCard.cardNumber", [
        Validators.required,
        Validators.pattern(/^\d{13,19}$/),
      ]);
      this.setValidators("paymentDetails.creditCard.expirationDate", [
        Validators.required,
        Validators.pattern(/^(0[1-9]|1[0-2])\/(\d{2})$/),
      ]);
      this.setValidators("paymentDetails.creditCard.cardHolder", [
        Validators.required,
        Validators.minLength(2),
      ]);
      this.setValidators("paymentDetails.creditCard.cvc", [
        Validators.required,
        Validators.pattern(/^\d{3,4}$/),
      ]);
    }

    if (key === "paypal") {
      this.setValidators("paymentDetails.paypal.email", [
        Validators.required,
        Validators.email,
      ]);
    }

    if (key === "bitcoin") {
      this.setValidators("paymentDetails.bitcoin.walletAddress", [
        Validators.required,
        Validators.minLength(10),
      ]);
    }

    this.checkoutForm
      .get("paymentDetails")
      ?.updateValueAndValidity({ emitEvent: false });
  }

  private resetNonSelectedPaymentGroups(
    selectedKey: "credit-card" | "paypal" | "bitcoin" | "unknown",
  ): void {
    const ccGroup = this.checkoutForm.get(
      "paymentDetails.creditCard",
    ) as FormGroup | null;
    const paypalGroup = this.checkoutForm.get(
      "paymentDetails.paypal",
    ) as FormGroup | null;
    const btcGroup = this.checkoutForm.get(
      "paymentDetails.bitcoin",
    ) as FormGroup | null;

    if (selectedKey !== "credit-card") ccGroup?.reset({}, { emitEvent: false });
    if (selectedKey !== "paypal") paypalGroup?.reset({}, { emitEvent: false });
    if (selectedKey !== "bitcoin") btcGroup?.reset({}, { emitEvent: false });
  }

  private clearPaymentValidators(): void {
    this.clearValidators("paymentDetails.creditCard.cardNumber");
    this.clearValidators("paymentDetails.creditCard.expirationDate");
    this.clearValidators("paymentDetails.creditCard.cardHolder");
    this.clearValidators("paymentDetails.creditCard.cvc");

    this.clearValidators("paymentDetails.paypal.email");
    this.clearValidators("paymentDetails.bitcoin.walletAddress");
  }

  private setValidators(path: string, validators: any[]): void {
    const control = this.checkoutForm.get(path);
    if (!control) return;
    control.setValidators(validators);
    control.updateValueAndValidity({ emitEvent: false });
  }

  private clearValidators(path: string): void {
    const control = this.checkoutForm.get(path);
    if (!control) return;
    control.clearValidators();
    control.updateValueAndValidity({ emitEvent: false });
  }

  isInvalid(path: string): boolean {
    const control = this.checkoutForm.get(path);
    return !!control && control.invalid && (control.touched || control.dirty);
  }

  onSubmit(carId: string): void {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    this.isLoadingSubject.next(true);
    this.errorMessageSubject.next("");

    const formValue = this.checkoutForm.value;

    const request = {
      carId: carId,
      pickUpLocationId: Number(formValue.rentalInfo.pickUpLocationId),
      dropOffLocationId: Number(formValue.rentalInfo.dropOffLocationId),
      pickUpDate: new Date(formValue.rentalInfo.pickUpDate).toISOString(),
      dropOffDate: new Date(formValue.rentalInfo.dropOffDate).toISOString(),
      paymentMethodId: Number(formValue.paymentMethodId),
      promoCode: "", // Optional
    };

    this.rentalService
      .createRental(request)
      .pipe(finalize(() => this.isLoadingSubject.next(false)))
      .subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.checkoutForm
              .get("paymentDetails")
              ?.reset({}, { emitEvent: false });

            this.router.navigate(["/"], {
              queryParams: { rentalSuccess: 1 },
            });
          } else {
            this.errorMessageSubject.next(res.message || "Booking failed.");
          }
        },
        error: (err) => {
          this.errorMessageSubject.next(
            err.error?.message || "An unexpected error occurred.",
          );
        },
      });
  }
}
