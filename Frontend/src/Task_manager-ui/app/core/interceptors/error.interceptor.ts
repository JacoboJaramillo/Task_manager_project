import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from "rxjs";


export const errorInterceptor: HttpInterceptorFn = (req, next) =>{
    const snackBar = inject(MatSnackBar);
    return next(req).pipe(
        catchError((error) =>{
            switch(error.status){
                case 0:
                    snackBar.open('No se puede conectar al servidor', 'Cerrar', {duration: 3000})
                    break;
                case 401:
                    snackBar.open('Registro no valido', 'Cerrar', {duration: 3000})
                    break;
                case 409:
                    snackBar.open('Correo ya registrado en otra cuenta', 'Cerrar', {duration: 3000})
                    break;
                case 500:
                    snackBar.open('Error inesperado del servidor, tranquilo, no es tu culpa', 'Cerrar', {duration: 3000})
                    break;
                default:
                    snackBar.open('Error', 'Cerrar', {duration: 3000})
                }
                

            return  throwError(() => error)
        })
    )
}