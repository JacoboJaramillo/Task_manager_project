import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from '../src/Task_manager-ui/app/app.config';
import { App } from '../src/Task_manager-ui/app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
