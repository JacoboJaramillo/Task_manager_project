import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { User } from '../models/user.model';
import { enviroment } from '../../../../enviroments/enviroments.developtment';
import { Observable, tap } from 'rxjs';
import { LoginDTO } from '../models/loginDto.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {


  constructor(private http: HttpClient){

  }

  loginUsuario(credenciales: LoginDTO): Observable<any>{
    return this.http.post<any>(`${enviroment.apiURL}/User/login`, credenciales).pipe(
      tap(resp =>{
        if(resp && resp.token){
          sessionStorage.setItem('jwt_token', resp.token);
        }
      })
    )
    }
  
  registrarUsuario(credenciales: User): Observable<any>{
    return this.http.post<any>(`${enviroment.apiURL}/User`, credenciales)
  }

  getToken(): string | null {
    return sessionStorage.getItem('jwt_token');
  }

  isLoggedIn(): boolean{
    return this.getToken() !== null;
  }

  logOut(): void{
    sessionStorage.removeItem('jwt_token')
  }

  }
