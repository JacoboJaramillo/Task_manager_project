import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { TaskListComponent } from './features/tasks/task-list/task-list.component';
import { AuthGuard } from './core/guards/auth.guard';
import { RegisterComponent } from './features/auth/register/register.component';
import { TaskFormComponent } from './features/tasks/task-form/task-form.component/task-form.component';
import { DashboardComponent } from './features/dashboard/dashboard.component/dashboard.component';

export const routes: Routes = [
    {path: '', redirectTo: 'tasks', pathMatch: 'full'},
    {path: 'login', component: LoginComponent },
    {path: 'register', component: RegisterComponent},
    {path: 'tasks', component: TaskListComponent, canActivate: [AuthGuard]},
    {path: 'task-form', component: TaskFormComponent, canActivate: [AuthGuard]},
    {path: 'task-form/:id', component: TaskFormComponent, canActivate: [AuthGuard]},
    {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
    
    
]