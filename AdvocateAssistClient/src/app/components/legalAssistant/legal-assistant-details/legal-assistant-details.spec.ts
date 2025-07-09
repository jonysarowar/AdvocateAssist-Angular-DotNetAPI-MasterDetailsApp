import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalAssistantDetails } from './legal-assistant-details';

describe('LegalAssistantDetails', () => {
  let component: LegalAssistantDetails;
  let fixture: ComponentFixture<LegalAssistantDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LegalAssistantDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LegalAssistantDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
