import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { AuthRoutingModule } from "./auth-routing-module";
import { Login } from "./pages/login/login";
import { Register } from "./pages/register/register";
import { SharedModule } from "../../shared/shared-module";
import { ReactiveFormsModule } from "@angular/forms";

@NgModule({
  declarations: [Login, Register],
  imports: [CommonModule, ReactiveFormsModule, AuthRoutingModule, SharedModule],
})
export class AuthModule {}
