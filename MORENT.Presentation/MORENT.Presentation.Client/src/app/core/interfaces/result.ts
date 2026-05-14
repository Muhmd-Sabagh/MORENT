export interface IResult<T> {
  isSuccess: boolean;
  message: string;
  dataObject: T;
  errors: string[];
}

export interface IPagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
