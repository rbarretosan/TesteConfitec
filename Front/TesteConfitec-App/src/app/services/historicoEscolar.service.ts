import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { HistoricoEscolar } from '@app/models/HistoricoEscolar';
import { environment } from '@environments/environment';

import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable()
export class HistoricoEscolarService {
  baseURL = environment.apiURL + 'api/HistoricosEscolares';

  constructor(private http: HttpClient) {}

  public getHistoricosEscolaresByUsuarioId(usuarioId: number): Observable<HistoricoEscolar[]> {
    return this.http.get<HistoricoEscolar[]>(`${this.baseURL}/${usuarioId}`).pipe(take(1));
  }

  public saveHistoricoEscolar(usuarioId: number, historicosEscolares: HistoricoEscolar[]): Observable<HistoricoEscolar[]> {
    return this.http
      .put<HistoricoEscolar[]>(`${this.baseURL}/${usuarioId}`, historicosEscolares)
      .pipe(take(1));
  }

  public deleteHistoricoEscolar(usuarioId: number, historicoEscolarId: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${usuarioId}/${historicoEscolarId}`)
      .pipe(take(1));
  }

  public postUpload(usuarioId: number, historicoEscolarId: number, file: File): Observable<HistoricoEscolar>{
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http
              .post<HistoricoEscolar>(`${this.baseURL}/upload-file?usuarioId=${usuarioId}&historicoEscolarId=${historicoEscolarId}`, formData)
              .pipe(take(1));
  }
}
