import { Routes } from '@angular/router';
import { LegalAssistantDetails } from './components/legalAssistant/legal-assistant-details/legal-assistant-details';
import { LegalAssistantForm } from './components/legalAssistant/legal-assistant-form/legal-assistant-form';
import { LegalAssistantList } from './components/legalAssistant/legal-assistant-list/legal-assistant-list';
import { Home } from './home/home';

export const routes: Routes = [
  { path: '', component: Home, pathMatch: 'full' },
  // { path: '', component:LegalAssistantList, pathMatch: 'full' },
  { path: 'legalassistants', component: LegalAssistantList },
  { path: 'details/:id', component: LegalAssistantDetails },
  { path: 'create', component: LegalAssistantForm },
  { path: 'edit/:id', component: LegalAssistantForm },
  { path: '**', redirectTo: '' },
];
