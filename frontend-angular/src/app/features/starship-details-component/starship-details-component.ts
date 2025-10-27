import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StarshipStore } from '../../core/stores/starship.store';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner-component/loading-spinner-component';
import { Currency, ModificationType } from '../../core/models/starship.model';

@Component({
  selector: 'app-starship-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, LoadingSpinnerComponent],
  templateUrl: './starship-details-component.html',
  styleUrls: ['./starship-details-component.scss']
})
export class StarshipDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly store = inject(StarshipStore);

  starship = this.store.selectedStarship;
  loading = this.store.loading;
  error = this.store.error;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      
      // Get query params for currency and modifications
      const queryParams = this.route.snapshot.queryParams;
      const currency = queryParams['currency'] as Currency | undefined;
      const modificationsParam = queryParams['modifications'] as string;
      const modifications = modificationsParam 
        ? modificationsParam.split(',').map(m => m as ModificationType)
        : undefined;

      this.store.loadStarshipDetails(id, currency, modifications);
    });
  }

  ngOnDestroy(): void {
    this.store.clearSelectedStarship();
  }

  goBack(): void {
    this.router.navigate(['/starships']);
  }
}