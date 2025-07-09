import { TestBed } from '@angular/core/testing';

import { LegalassistantService } from './legalassistant-service';

describe('LegalassistantService', () => {
  let service: LegalassistantService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LegalassistantService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
