import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CarDetails } from "./pages/car-details/car-details";
import { Catalog } from "./pages/catalog/catalog";
import { authGuard } from "../../core/guards/auth.guard";

const routes: Routes = [
  { path: "details/:id", component: CarDetails },
  { path: "catalog", component: Catalog, canActivate: [authGuard] },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CarsRoutingModule {}
