import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/Auth.service';
import { Router, RouterLink } from '@angular/router';
import { LoginDTO } from '../../../core/models/loginDto.model';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-login.component',
  imports: [NgClass, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {

  servicioAuth = inject(AuthService)
  router = inject(Router)

  credenciales: LoginDTO ={ 
    email: '',
    password: ''
  };


  loginUsuario(){

    this.servicioAuth.loginUsuario(this.credenciales).subscribe({
      next: (resp) =>{
        console.log("Login exitoso")
        this.router.navigate(['tasks'])
      },
      error: (err) =>{
        console.error("Login invalido")
      }
    })
  }
}
