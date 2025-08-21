import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/user.dto';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private api: ApiService) { }

  list(page = 1, pageSize = 20) {
    return this.api.http.get<{ data: UserDto[]; total: number }>(`${this.api.base}/Users/GetAll`, { params: { page, pageSize } })
      .pipe(
        map(res => ({
          users: (res.data ?? []).map(x => ({
            id: x.id,
            fullName: x.fullName,
            email: x.email,
            roleId: x.roleId,
            role: x.role,
            isActive: x.isActive
          })) as UserDto[],
          total: res.total,
        }))
      );
  }
  get(id: number) {
    return this.api.http.get<UserDto>(`${this.api.base}/Users/Get/${id}`);
  }
  create(payload: any) {
    return this.api.http.post<UserDto>(`${this.api.base}/Users/Create`, payload);
  }
  update(id: number, payload: any) {
    return this.api.http.post(`${this.api.base}/Users/Update/${id}`, payload);
  }
  delete(id: number) {
    return this.api.http.post(`${this.api.base}/Users/Delete/${id}`, {});
  }
}
