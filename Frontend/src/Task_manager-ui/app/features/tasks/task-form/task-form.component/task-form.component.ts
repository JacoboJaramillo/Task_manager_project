import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-task-form.component',
  imports: [],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskFormComponent {}
