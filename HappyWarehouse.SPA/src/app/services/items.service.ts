import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { WarehouseItemDto, WarehouseItemFormModel } from '../models/warehouse-item.dto';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {

  constructor(private api: ApiService) { }

  listByWarehouse(warehouseId: number, page = 1, pageSize = 20) {
    return this.api.http.get<{ warehouses: any[]; total: number }>(
      `${this.api.base}/WarehouseItems/GetAll`,
      { params: { warehouseId, page, pageSize } }
    ).pipe(
      map(res => {
        const wharehouse = (res.warehouses ?? [])[0] ?? {};
        const itemsArr = wharehouse.warehouseItems ?? [];
        const items: WarehouseItemDto[] = itemsArr.map((i: any) => ({
          id: i.id,
          itemName: i.itemName,
          skuCode: i.skuCode,
          qty: i.qty,
          costPrice: i.costPrice,
          msrpPrice: i.msrpPrice,
          warehouseId,
          warehouseName: wharehouse.name ?? ''
        }));
        return { items, total: res.total, warehouseName: wharehouse.name ?? '' };
      })
    );
  }

  get(id: number) {
    return this.api.http.get<WarehouseItemDto>(`${this.api.base}/WarehouseItems/Get/${id}`);
  }

  getForEdit(id: number) {
    return this.get(id).pipe(
      map(x => ({
        itemName: x.itemName ?? '',
        skuCode: x.skuCode ?? '',
        qty: x.qty,
        costPrice: x.costPrice,
        msrpPrice: x.msrpPrice ?? 0
      } as WarehouseItemFormModel))
    );
  }

  createForWarehouse(payload: {
    warehouseId: number; itemName: string; skuCode?: string | null;
    qty: number; costPrice: number; msrpPrice?: number | null;
  }) {
    return this.api.http.post<WarehouseItemDto>(`${this.api.base}/WarehouseItems/Create`, payload);
  }

  update(id: number, payload: any) {
    return this.api.http.post(`${this.api.base}/WarehouseItems/Update/${id}`, payload);
  }

  delete(id: number) {
    return this.api.http.post(`${this.api.base}/WarehouseItems/Delete/${id}`, {});
  }
}
