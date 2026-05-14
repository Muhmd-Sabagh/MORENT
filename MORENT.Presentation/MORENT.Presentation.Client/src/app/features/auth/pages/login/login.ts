import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../../../core/services/auth.service";
import { BehaviorSubject, finalize } from "rxjs";

@Component({
  selector: "app-login",
  templateUrl: "./login.html",
  styleUrls: ["./login.css"],
  standalone: false,
})
export class Login {
  loginForm: FormGroup;

  private readonly isLoadingSubject = new BehaviorSubject<boolean>(false);
  readonly isLoading$ = this.isLoadingSubject.asObservable();

  private readonly errorMessageSubject = new BehaviorSubject<string>("");
  readonly errorMessage$ = this.errorMessageSubject.asObservable();

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.loginForm = this.fb.group({
      username: ["", Validators.required],
      password: ["", Validators.required],
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isLoadingSubject.next(true);
    this.errorMessageSubject.next("");

    this.authService
      .login(this.loginForm.value)
      .pipe(finalize(() => this.isLoadingSubject.next(false)))
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            // The HttpOnly cookie is securely set by the browser automatically!
            const userRole = response.dataObject?.role;
            const returnUrl =
              this.route.snapshot.queryParamMap.get("returnUrl") || "/";

            if (userRole === "Admin") {
              const adminTarget = returnUrl.startsWith("/dashboard")
                ? returnUrl
                : "/dashboard";
              this.router.navigate([adminTarget]);
              return;
            }

            const clientTarget = returnUrl.startsWith("/dashboard")
              ? "/"
              : returnUrl;
            this.router.navigate([clientTarget]);
            return;
          }

          this.errorMessageSubject.next(response.message || "Login failed.");
        },
        error: (err) => {
          this.errorMessageSubject.next(
            err.error?.message || "An unexpected error occurred.",
          );
        },
      });
  }
}
