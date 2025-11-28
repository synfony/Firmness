// Estos tipos deben coincidir con los ViewModels de tu API de ASP.NET Core

export interface LoginViewModel {
  Email: string;
  Password: string;
  RememberMe?: boolean;
}

export interface RegisterViewModel {
  Email: string;
  Password: string;
  ConfirmPassword?: string;
}

export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
}

export interface CartItem extends ProductDto {
  quantity: number;
}

// Tipos para crear una nueva venta
export interface SaleDetailDto {
  productId: number;
  quantity: number;
  unitPrice: number;
}

export interface SaleDto {
  clientId: number; // El ID del cliente que realiza la compra
  saleDetails: SaleDetailDto[];
}
