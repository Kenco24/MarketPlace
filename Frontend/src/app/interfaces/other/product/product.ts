export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  categoryName: string;
  isSold: boolean;
  image : string;
  sellerFullName: string;
}
