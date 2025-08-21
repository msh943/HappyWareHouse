import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpParams } from '@angular/common/http';
import { map, catchError, of, Observable } from 'rxjs';
import { LogRowDto } from '../models/rawlog.dto';

@Injectable({
  providedIn: 'root'
})
export class LogsviewService {

  constructor(private api: ApiService) { }

  get(last = 200, level?: string, search?: string): Observable<LogRowDto[]> {
    let params = new HttpParams().set('last', String(last));
    if (level) params = params.set('level', level);
    if (search) params = params.set('search', search);

    return this.api.http
      .get<LogsResponse>(`${this.api.base}/Logs/GetLogs`, { params })
      .pipe(
        map(r => r?.items ?? []),
        catchError(() => of<LogRowDto[]>([]))
      );
  }
}

interface LogsResponse {
  items: LogRowDto[];
}


