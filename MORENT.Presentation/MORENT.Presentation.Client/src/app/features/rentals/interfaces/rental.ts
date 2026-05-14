export interface ICreateRentalRequest {
  carId: string;
  pickUpLocationId: number;
  dropOffLocationId: number;
  pickUpDate: string;
  dropOffDate: string;
  paymentMethodId: number;
  promoCode?: string;
}

export interface IRentalDto {
  id: string;
  carBrand: string;
  pickUpLocation: string;
  dropOffLocation: string;
  pickUpDate: string;
  dropOffDate: string;
  rentalStatus: string;
  totalAmount: number;
}
