import { Injectable } from '@angular/core';
import { WarehouseDto } from '../models/warehouse.dto';
import { ApiService } from './api.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WarehousesService {

  constructor(private api: ApiService) { }

  getAll(page = 1, pageSize = 10) {
    return this.api.http
      .get<{ data: any[]; total: number }>(`${this.api.base}/Warehouses/GetAll`, {
        params: { page, pageSize },
      })
      .pipe(
        map(res => ({
          warehouses: (res.data ?? []).map(x => ({
            id: x.id,
            name: x.name,
            address: x.address,
            city: x.city,
            countryName: x.country ?? x.Country ?? '',
            countryId: x.countryId ?? null,
          })) as WarehouseDto[],
          total: res.total,
        }))
      );
  }
  get(id: number) { return this.api.http.get<WarehouseDto>(`${this.api.base}/Warehouses/Get/${id}`); }
  create(payload: any) { return this.api.http.post<WarehouseDto>(`${this.api.base}/Warehouses/Create`, payload); }
  update(id: number, payload: any) { return this.api.http.post(`${this.api.base}/Warehouses/Update/${id}`, payload); }
  delete(id: number) { return this.api.http.post(`${this.api.base}/Warehouses/Delete/${id}`, {}); }
}
