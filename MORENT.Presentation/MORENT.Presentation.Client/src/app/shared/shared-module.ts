import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Button } from "./componentes/button/button";
import { CarCard } from "./componentes/car-card/car-card";

@NgModule({
  declarations: [Button, CarCard],
  imports: [CommonModule],
  exports: [Button, CarCard],
})
export class SharedModule {}
