import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { correlationIdInterceptor } from './core/interceptors/correlation-id-interceptor';
import { errorHandlerInterceptor } from './core/interceptors/error-handler-interceptor';
import { loadingInterceptor } from './core/interceptors/loading-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        correlationIdInterceptor,
        errorHandlerInterceptor,
        loadingInterceptor
      ])
    )
  ]
};
