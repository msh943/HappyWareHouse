export interface WarehouseItemDto {

  id: number;
  itemName: string;
  skuCode?: string | null;
  qty: number;
  costPrice: number;
  msrpPrice?: number | null;
  warehouseId: number;
  warehouseName: string;

}

export interface WarehouseItemFormModel {
  itemName: string;
  skuCode: string;
  qty: number;
  costPrice: number;
  msrpPrice: number;
}
