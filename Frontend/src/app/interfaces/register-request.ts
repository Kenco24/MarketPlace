export interface RegisterRequest {
    email: string;
    fullName: string;
    country: string;
    city: string;
    address: string;
    password: string;
    roles?: string[]; 
  }
  