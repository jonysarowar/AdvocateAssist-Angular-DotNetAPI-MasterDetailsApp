import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Case } from '../../../models/legalAssistant/case';
import { LegalAssistant } from '../../../models/legalAssistant/legalassistant';
import { LegalassistantService } from '../../../services/legalAssistant/legalassistant-service';

@Component({
  selector: 'app-legal-assistant-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './legal-assistant-list.html',
  styleUrl: './legal-assistant-list.css',
})
export class LegalAssistantList implements OnInit {
  legalAssistants: LegalAssistant[] = [];
  cases: Case[] = [];

  imageUrlBase = 'http://localhost:5268/images/';

  constructor(private service: LegalassistantService, private router: Router) {}

  ngOnInit(): void {
    this.service.getCases().subscribe({
      next: (cases) => {
        this.cases = cases;
        this.loadCasesAndAssistants();
      },
      error: (err) => {
        alert('Failed to load case data.');
        this.loadCasesAndAssistants();
      },
    });
  }

  loadCasesAndAssistants(): void {
    this.service.getLegalAssistants().subscribe({
      next: (data) => {
        this.legalAssistants = data.map((c) => {
          c.legalAssistantCases = c.legalAssistantCases.map((cs) => {
            let caseNumberFromBackend = cs.case?.caseNumber;
            let caseNumberFromLookup = '';
            if (!caseNumberFromBackend) {
              const foundCase = this.cases.find((s) => s.caseId === cs.caseId);
              caseNumberFromLookup = foundCase
                ? foundCase.caseNumber
                : 'Unknown Case';
            }
            return {
              ...cs,
              casenumber: caseNumberFromBackend || caseNumberFromLookup,
            };
          });

          return c;
        });
      },
      error: (err) => {
        alert('Failed to load legalAssistant.');
      },
    });
  }

  deleteCandidate(id: number): void {
    if (confirm('Are you sure you want to delete this assistant?')) {
      this.service.deleteLegalAssistant(id).subscribe({
        next: () => { console.log(`Deleted successfully.`);
          this.legalAssistants = this.legalAssistants.filter(c => c.legalAssistantId !== id);
        },
        error: (err) => { alert('Failed to delete.');}
      });
    }
  }
}
