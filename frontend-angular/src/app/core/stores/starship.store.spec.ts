import { TestBed } from '@angular/core/testing';

import { Starship } from './starship';

describe('Starship', () => {
  let service: Starship;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Starship);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
