import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../../shared/shared-module";
import { RentalsRoutingModule } from "./rentals-routing-module";
import { Checkout } from "./pages/checkout/checkout";
import { MyRentals } from "./pages/my-rentals/my-rentals";

@NgModule({
  declarations: [Checkout, MyRentals],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    RentalsRoutingModule,
  ],
})
export class RentalsModule {}
