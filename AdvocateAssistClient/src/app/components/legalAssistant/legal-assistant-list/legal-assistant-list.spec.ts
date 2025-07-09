import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalAssistantList } from './legal-assistant-list';

describe('LegalAssistantList', () => {
  let component: LegalAssistantList;
  let fixture: ComponentFixture<LegalAssistantList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LegalAssistantList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LegalAssistantList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
