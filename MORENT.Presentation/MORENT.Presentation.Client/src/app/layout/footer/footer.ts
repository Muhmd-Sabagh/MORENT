import { Component } from "@angular/core";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.html",
  standalone: false,
})
export class Footer {
  currentYear: number = new Date().getFullYear();
}
