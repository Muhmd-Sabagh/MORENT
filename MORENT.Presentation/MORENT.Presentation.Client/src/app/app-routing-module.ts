import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {
    path: "",
    loadChildren: () =>
      import("./features/home/home-module").then((m) => m.HomeModule),
  },
  {
    path: "auth",
    loadChildren: () =>
      import("./features/auth/auth-module").then((m) => m.AuthModule),
  },
  {
    path: "cars",
    loadChildren: () =>
      import("./features/cars/cars-module").then((m) => m.CarsModule),
  },
  {
    path: "rentals",
    loadChildren: () =>
      import("./features/rentals/rentals-module").then((m) => m.RentalsModule),
  },
  {
    path: "dashboard",
    loadChildren: () =>
      import("./features/dashboard/dashboard-module").then(
        (m) => m.DashboardModule,
      ),
  },
  { path: "**", redirectTo: "" },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
