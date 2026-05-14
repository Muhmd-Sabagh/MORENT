import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
  selector: "app-button",
  templateUrl: "./button.html",
  styleUrls: ["./button.css"],
  standalone: false,
})
export class Button {
  @Input() label: string = "Button";
  @Input() variant: "primary" | "secondary" | "minimal" = "primary";
  @Input() size: "small" | "medium" | "large" = "medium";
  @Input() disabled: boolean = false;
  @Input() iconLeft?: string;
  @Input() iconRight?: string;
  @Input() fullWidth: boolean = false;

  @Output() onClick = new EventEmitter<Event>();

  get classes(): string {
    let baseClasses =
      "btn d-inline-flex align-items-center justify-content-center gap-2 font-semibold custom-btn ";

    // Size Classes (From Typography.pdf)
    if (this.size === "small") baseClasses += "btn-sm text-xs px-3 py-2 ";
    if (this.size === "medium") baseClasses += "text-sm px-4 py-2 ";
    if (this.size === "large") baseClasses += "btn-lg text-base px-5 py-3 ";

    // Variant Classes
    if (this.variant === "primary") baseClasses += "btn-primary ";
    if (this.variant === "secondary") baseClasses += "btn-outline-primary ";
    if (this.variant === "minimal")
      baseClasses += "btn-link text-muted text-decoration-none ";

    if (this.fullWidth) baseClasses += "w-100 ";

    return baseClasses;
  }
}
