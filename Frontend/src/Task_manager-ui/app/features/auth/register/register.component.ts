import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/Auth.service';
import { Router, RouterLink } from '@angular/router';
import { User } from '../../../core/models/user.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register.component',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {

  servicioAuth = inject(AuthService)
  router = inject(Router)
  errorMsg: string = ''

  credenciales: User ={
    name: '',
    email: '',
    password: ''
  };

  registrarUsuario(){
    this.servicioAuth.registrarUsuario(this.credenciales).subscribe({
      next: (resp)=>{
        console.log("Registro exitoso")
        this.router.navigate(['login'])
      },
      error: (err)=>{
        this.errorMsg = 'El correo ya esta asignado a otra cuenta'
        console.error("registro invalido")
      }
    })
  }
}
