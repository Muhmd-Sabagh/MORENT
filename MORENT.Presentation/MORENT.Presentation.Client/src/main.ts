import {
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
} from "@angular/core";
import { bootstrapApplication } from "@angular/platform-browser";
import { provideZonelessChangeDetection } from "@angular/core";

import { App } from "./app/app";
import { AppRoutingModule } from "./app/app-routing-module";
import { CoreModule } from "./app/core/core-module";

bootstrapApplication(App, {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    importProvidersFrom(CoreModule, AppRoutingModule),
  ],
}).catch((err) => console.error(err));
