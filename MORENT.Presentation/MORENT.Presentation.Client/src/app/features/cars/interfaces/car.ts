export interface ICarDto {
  id: string;
  brand: string;
  carType: string;
  fuelType: string;
  steeringType: string;
  capacity: number;
  pricePerDay: number;
  discount: number | null;
  mainImageUrl: string;
  averageRating: number;
}
