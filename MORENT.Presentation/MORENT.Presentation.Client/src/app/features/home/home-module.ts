import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { Home } from "./home/home";
import { SharedModule } from "../../shared/shared-module";
import { HomeRoutingModule } from "./home-routing-module";

@NgModule({
  declarations: [Home],
  imports: [CommonModule, SharedModule, HomeRoutingModule],
  exports: [Home],
})
export class HomeModule {}
