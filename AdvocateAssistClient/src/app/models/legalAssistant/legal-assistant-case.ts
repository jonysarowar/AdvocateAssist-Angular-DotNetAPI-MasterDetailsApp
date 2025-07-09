import { Case } from './case';

export interface LegalAssistantCase {
  legalAssistantId?: number;
  caseId: number;
  case?: Case;
  caseNumber?: string;
  caseTitle: string;
  firnumber?: string;
  filingDate: Date;
}
