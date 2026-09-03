import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { TaskService } from '../../../core/services/Task.service';

@Component({
  selector: 'app-dashboard.component',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {

  taskService = inject(TaskService)
  cdr = inject(ChangeDetectorRef)

  hoy = new Date()

  totaltareas: number = 0;
  tareasPendientes: number = 0;
  tareasCompletadas: number = 0;
  tareasVencidas: number = 0;
  tareasPrioridad = {Low: 0, Medium: 0, High: 0}

  ngOnInit() {
    this.taskService.getAll().subscribe(tarea =>{
      this.totaltareas = tarea.length;
      console.log('tareas totales:', this.totaltareas)

      this.tareasPendientes = tarea.filter(t => !t.isCompleted).length;
      console.log('tareas por hacer:', this.tareasPendientes)

      this.tareasCompletadas = tarea.filter(t => t.isCompleted).length;
      console.log('tareas completadas', this.tareasCompletadas)

      this.tareasVencidas = tarea.filter(t => t.dueDate && new Date(t.dueDate) < this.hoy && !t.isCompleted).length
      console.log('tareas vencidas', this.tareasVencidas)

      this.tareasPrioridad = {
        Low: tarea.filter(t => t.priority === 'Low' ).length,
        Medium: tarea.filter(t => t.priority === 'Medium').length,
        High: tarea.filter(t => t.priority === 'High').length,
      }
      console.log('Low', this.tareasPrioridad.Low)
      console.log('Medium', this.tareasPrioridad.Medium)
      console.log('High', this.tareasPrioridad.High)
    })
  }
}
