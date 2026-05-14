import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../core/services/auth.service";
import { IAuthResponse } from "../../core/interfaces/auth";
import { Observable } from "rxjs";

@Component({
  selector: "app-header",
  templateUrl: "./header.html",
  styleUrls: ["./header.css"],
  standalone: false,
})
export class Header implements OnInit {
  currentUser$: Observable<IAuthResponse | null>;

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    // We bind directly to the BehaviorSubject so the UI updates instantly on login/logout
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {}

  onLogout(): void {
    this.authService.logout().subscribe(() => {
      this.router.navigate(["/auth/login"]);
    });
  }
}
