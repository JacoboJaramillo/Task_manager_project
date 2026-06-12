import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/Task.service';
import { AsyncPipe, NgClass } from '@angular/common';
import { Router } from '@angular/router';


@Component({
  selector: 'app-task-list.component',
  imports: [AsyncPipe, NgClass],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css',
})
export class TaskListComponent  implements OnInit{

  taskService = inject(TaskService)
  route = inject(Router)
  tareas$!: Observable<Task[]>

  ngOnInit(){
   this.tareas$ = this.taskService.getAll()
  }

  editarTarea(tarea: Task){
    if(!tarea.id) return

    const tareaClon = {...tarea};
    console.log(tarea)
    this.route.navigate(['task-form', tareaClon.id])

  }


  eliminarTarea(id: string, tarea: Task){
    if(!id) return;

    if(confirm(`Estas seguro de eliminar la tarea ${tarea.title} definitivamente?`)){
      this.taskService.deleteTask(id).subscribe({
        next: (resp) =>{
         this.tareas$ = this.taskService.getAll()
        }
      })   
    }
  }
}
