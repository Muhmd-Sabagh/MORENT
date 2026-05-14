import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { CarsRoutingModule } from "./cars-routing-module";
import { CarDetails } from "./pages/car-details/car-details";
import { Catalog } from "./pages/catalog/catalog";
import { SharedModule } from "../../shared/shared-module";
import { ReactiveFormsModule } from "@angular/forms";

@NgModule({
  declarations: [CarDetails, Catalog],
  imports: [CommonModule, ReactiveFormsModule, CarsRoutingModule, SharedModule],
})
export class CarsModule {}
