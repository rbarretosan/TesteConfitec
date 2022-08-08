import { Usuario } from './../models/Usuario';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Injectable(
// { providedIn: 'root'}
)
export class UsuarioService {
  baseURL = environment.apiURL + 'api/usuarios';

  constructor(private http: HttpClient) { }

  public getUsuarios(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(this.baseURL).pipe(take(1));
  }

  public getUsuariosByTema(nome: string): Observable<Usuario[]> {
    return this.http
      .get<Usuario[]>(`${this.baseURL}/${nome}/nome`)
      .pipe(take(1));
  }

  public getUsuarioById(id: number): Observable<Usuario> {
    return this.http
      .get<Usuario>(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  public post(usuario: Usuario): Observable<Usuario> {
    return this.http
      .post<Usuario>(this.baseURL, usuario)
      .pipe(take(1));
  }

  public put(usuario: Usuario): Observable<Usuario> {
    return this.http
      .put<Usuario>(`${this.baseURL}/${usuario.id}`, usuario)
      .pipe(take(1));
  }

  public deleteUsuario(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }
}
