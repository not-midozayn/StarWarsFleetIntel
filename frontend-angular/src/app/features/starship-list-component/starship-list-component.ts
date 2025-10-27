import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { StarshipStore } from '../../core/stores/starship.store';
import { StarshipCardComponent } from './components/starship-card-component/starship-card-component';
import { SearchBarComponent } from '../../shared/components/search-bar-component/search-bar-component';
import { PaginationComponent } from '../../shared/components/pagination-component/pagination-component';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner-component/loading-spinner-component';
import { Currency, ModificationType } from '../../core/models/starship.model';

@Component({
  selector: 'app-starship-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    StarshipCardComponent,
    SearchBarComponent,
    PaginationComponent,
    LoadingSpinnerComponent
  ],
  templateUrl: './starship-list-component.html',
  styleUrls: ['./starship-list-component.scss']
})
export class StarshipListComponent implements OnInit {
  private readonly store = inject(StarshipStore);

  // Store state to template
  starships = this.store.filteredStarships;
  pagination = this.store.pagination;
  loading = this.store.loading;
  error = this.store.error;

  // Filter options
  currencies = Object.values(Currency);
  modifications = Object.values(ModificationType);

  // Computed signals that stay in sync with the store
  selectedCurrency = computed(() => this.store.filters().currency);
  selectedModifications = computed(() => this.store.filters().modifications);

  ngOnInit(): void {
    this.store.loadStarships();
  }

  onSearch(searchTerm: string): void {
    this.store.setSearchTerm(searchTerm);
  }

  onCurrencyChange(currency: string): void {
    const selectedCurrency = currency ? (currency as Currency) : null;
    this.store.setCurrency(selectedCurrency);
  }

  onModificationToggle(modification: ModificationType): void {
    this.store.toggleModification(modification);
  }

  onPageChange(page: number): void {
    this.store.loadStarships(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onNextPage(): void {
    this.store.nextPage();
  }

  onPreviousPage(): void {
    this.store.previousPage();
  }

  isModificationSelected(modification: ModificationType): boolean {
    return this.selectedModifications().includes(modification);
  }
}