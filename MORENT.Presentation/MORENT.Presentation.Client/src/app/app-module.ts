import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing-module";
import { CoreModule } from "./core/core-module";
import { LayoutModule } from "./layout/layout-module";

@NgModule({
  imports: [BrowserModule, AppRoutingModule, CoreModule, LayoutModule],
})
export class AppModule {}
