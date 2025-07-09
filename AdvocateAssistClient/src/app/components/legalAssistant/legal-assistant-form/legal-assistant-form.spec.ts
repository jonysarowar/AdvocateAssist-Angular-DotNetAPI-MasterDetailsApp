import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalAssistantForm } from './legal-assistant-form';

describe('LegalAssistantForm', () => {
  let component: LegalAssistantForm;
  let fixture: ComponentFixture<LegalAssistantForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LegalAssistantForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LegalAssistantForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
