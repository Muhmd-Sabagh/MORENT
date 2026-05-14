import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

import { LayoutModule } from "./layout/layout-module";

@Component({
  selector: "app-root",
  templateUrl: "./app.html",
  standalone: true,
  imports: [RouterOutlet, LayoutModule],
  styleUrls: ["./app.css"],
})
export class App {
  title = "MORENT Car Rental";
}
