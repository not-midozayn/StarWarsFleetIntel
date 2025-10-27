import { Routes } from '@angular/router';
import { StarshipListComponent } from './features/starship-list-component/starship-list-component';
import { StarshipDetailComponent } from './features/starship-details-component/starship-details-component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'starships',
    pathMatch: 'full'
  },
  {
    path: 'starships',
    component: StarshipListComponent
  },
  {
    path: 'starships/:id',
    component: StarshipDetailComponent
  },
  {
    path: '**',
    redirectTo: 'starships'
  }
];