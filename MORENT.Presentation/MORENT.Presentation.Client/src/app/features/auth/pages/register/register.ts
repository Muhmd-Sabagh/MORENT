import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../../../../core/services/auth.service";
import { BehaviorSubject, finalize } from "rxjs";

@Component({
  selector: "app-register",
  templateUrl: "./register.html",
  styleUrls: ["./register.css"],
  standalone: false,
})
export class Register {
  registerForm: FormGroup;

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(false);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  private readonly errorMessageSubject = new BehaviorSubject<string>("");
  readonly errorMessage$ = this.errorMessageSubject.asObservable();

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    this.registerForm = this.fb.group({
      firstName: ["", Validators.required],
      lastName: [""],
      email: ["", [Validators.email]],
      phoneNumber: [""],
      username: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    this.isLoadingSubject.next(true);
    this.errorMessageSubject.next("");

    this.authService
      .register(this.registerForm.value)
      .pipe(finalize(() => this.isLoadingSubject.next(false)))
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.router.navigate(["/"]); // Success! The cookie is set. Navigate to Home.
            return;
          }

          this.errorMessageSubject.next(
            response.message || "Registration failed.",
          );
        },
        error: (err) => {
          this.errorMessageSubject.next(
            err.error?.message || "An unexpected error occurred.",
          );
        },
      });
  }
}
