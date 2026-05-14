export interface ILoginRequest {
  username?: string;
  password?: string;
}

export interface IRegisterRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  username?: string;
  password?: string;
  phoneNumber?: string;
}

export interface IAuthResponse {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  token: string;
  role: string;
}
