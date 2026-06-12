import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { TaskService } from '../../../../core/services/Task.service';
import { Task } from '../../../../core/models/task.model';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-form.component',
  imports: [FormsModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskFormComponent implements OnInit {

  activeRoute = inject(ActivatedRoute)
  route = inject(Router)
  taskService = inject(TaskService)
  cdr = inject(ChangeDetectorRef)

  nuevaTarea: Partial<Task> = {
    title: '',
    description: '',
    isCompleted: false,
    priority: '',
    dueDate: null as string | null
  };

  ngOnInit(){
    const tareaId = this.activeRoute.snapshot.paramMap.get('id')
    console.log('tareaId', tareaId)
    if(tareaId != null){
      this.taskService.getId(tareaId).subscribe(tarea =>{
        this.nuevaTarea = {...tarea,
          dueDate: tarea.dueDate ? new Date(tarea.dueDate).toISOString().substring(0, 10): undefined
        }
        this.cdr.detectChanges()

        console.log('tarea recibida', tarea)
      })
      
    }
 
  }

  crearTarea(){
    if(this.nuevaTarea.id == null){
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
    else{
      this.taskService.updateTask(this.nuevaTarea.id, this.nuevaTarea).subscribe({
        next: (resp) =>{
          console.log("tarea editandose")
          this.route.navigate(['tasks'])
        }
      })
    }


  }
}

//TODO: HACER EL TEMPLATE