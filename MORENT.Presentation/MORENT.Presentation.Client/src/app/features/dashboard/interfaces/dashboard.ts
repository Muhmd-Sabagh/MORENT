export interface IDashboardData {
  activeRental?: IActiveRental;
  totalRentals: number;
  topCars: ICarStat[];
  recentTransactions: IRecentTransaction[];
}

export interface IActiveRental {
  carName: string;
  carType: string;
  imageUrl: string;
  pickUpLocation: string;
  pickUpDate: string; // ISO string mapped natively from EF Core DateTime
  dropOffLocation: string;
  dropOffDate: string; // ISO string
  totalPrice: number;
}

export interface ICarStat {
  type: string;
  count: number;
  colorHex: string;
}

export interface IRecentTransaction {
  id: string;
  carName: string;
  carType: string;
  date: string; // ISO string mapped natively from EF Core DateTime
  price: number;
  imageUrl: string;
}
