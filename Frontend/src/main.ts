import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './Task_manager-ui/app/app.config';
import { App } from './Task_manager-ui/app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
