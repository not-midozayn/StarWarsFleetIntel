import { TestBed } from '@angular/core/testing';

import { StarshipApi } from './starship-api';

describe('StarshipApi', () => {
  let service: StarshipApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StarshipApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
