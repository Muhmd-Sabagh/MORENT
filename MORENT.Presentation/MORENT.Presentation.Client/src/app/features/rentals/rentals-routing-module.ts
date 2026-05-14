import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { Checkout } from "./pages/checkout/checkout";
import { MyRentals } from "./pages/my-rentals/my-rentals";
import { authGuard } from "../../core/guards/auth.guard";

const routes: Routes = [
  { path: "checkout/:id", component: Checkout, canActivate: [authGuard] },
  { path: "my-rentals", component: MyRentals, canActivate: [authGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RentalsRoutingModule {}
