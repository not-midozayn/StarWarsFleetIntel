import { Component, Input, computed, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Starship, Currency, ModificationType } from '../../../../core/models/starship.model';

@Component({
  selector: 'app-starship-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './starship-card-component.html',
  styleUrls: ['./starship-card-component.scss']
})
export class StarshipCardComponent {
  @Input({ required: true }) starship!: Starship;
  @Input() currency: Currency | null = null;
  @Input() modifications: ModificationType[] = [];

  private readonly router = inject(Router);

  starshipId = computed(() => {
    if (!this.starship?.id) {
      console.error('Starship or ID is missing:', this.starship);
      return null;
    }
    return this.starship.id.toString();
  });

  displayCost = computed(() => {
    if (this.starship.costConverted && this.starship.currency) {
      return `${this.starship.costConverted.toLocaleString()} ${this.starship.currency}`;
    }
    return this.starship.costInCredits !== 'unknown' 
      ? `${this.starship.costInCredits} Credits`
      : 'Price Unknown';
  });

  navigateToDetails(): void {
    console.log('Navigating to details...');
    const id = this.starshipId();
    console.log('Starship ID:', id);
    
    if (!id) {
      console.error('Cannot navigate: Invalid starship ID');
      return;
    }

    try {
      const queryParams: { [key: string]: string } = {};
      
      if (this.currency) {
        queryParams['currency'] = this.currency;
      }
      
      if (this.modifications.length > 0) {
        queryParams['modifications'] = this.modifications.join(',');
      }

      console.log('Navigating to:', ['starships', id], 'with params:', queryParams);
      
      this.router.navigate(['starships', id], {
        queryParams,
        queryParamsHandling: 'merge'
      }).then(success => {
        if (!success) {
          console.error('Navigation failed');
        }
      }).catch(error => {
        console.error('Navigation error:', error);
      });
    } catch (error) {
      console.error('Error during navigation:', error);
    }
  }

  onCardClick(event: MouseEvent): void {
    console.log('Card clicked');
    const target = event.target as HTMLElement;
    
    if (target.closest('.btn') || 
        target.tagName.toLowerCase() === 'button' || 
        target.tagName.toLowerCase() === 'a') {
      console.log('Clicked on button/link - skipping navigation');
      return;
    }

    console.log('Proceeding with navigation');
    this.navigateToDetails();
  }
}