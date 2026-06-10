import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TaskService } from '../../../../core/services/Task.service';
import { Task } from '../../../../core/models/task.model';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-form.component',
  imports: [FormsModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskFormComponent {

  taskService = inject(TaskService)
  route = inject(Router)

  nuevaTarea: Partial<Task> = {
    title: '',
    description: '',
    isCompleted: false,
    priority: '',
    dueDate: null as Date | null
  };

  crearTarea(){
    this.taskService.createTask(this.nuevaTarea).subscribe({
      next: (resp) =>{
        console.log("tarea creada exitosamente")
        this.route.navigate(['tasks'])
      },
      error: (err) =>{
        console.error("Fallo en creacion de tarea")
      }
    })
  }
}

//TODO: HACER EL TEMPLATE