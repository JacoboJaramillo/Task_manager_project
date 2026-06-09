import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { enviroment } from '../../../../enviroments/enviroments.developtment';
import { Task } from '../models/task.model';
@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private http = inject(HttpClient)
  private url = `${enviroment.apiURL}/Task`
  //METODO GET
  getAll()
  { 
    return this.http.get<Task[]>(this.url);
  }
  //METODO POST
  createTask(task: Partial<Task>) 
  {
    return this.http.post<Task>(this.url, task);
  }

  //METODO UPDATE
  updateTask(id: string, task: Partial<Task>) 
  {
    return this.http.put<Task>(`${this.url}/${id}`, task);
  }

  //METODO DELETE
  deleteTask(id: string) 
  {
    return this.http.delete(`${this.url}/${id}`);
  }



}
