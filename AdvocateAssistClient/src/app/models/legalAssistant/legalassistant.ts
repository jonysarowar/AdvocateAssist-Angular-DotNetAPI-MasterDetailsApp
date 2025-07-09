import { LegalAssistantCase } from "./legal-assistant-case";

export interface LegalAssistant {
  legalAssistantId: number;
  legalAssistantFname: string;
  legalAssistantLname: string;
  joinDate: Date;
  email: string;
  mobileNo: string;
  isActive: boolean;
  monthlyStipend?: number;
  // nidNumber: number;
  nidNumber: string;
  picture: string | null;
  barLicenseNumber?: string;

  legalAssistantCases: LegalAssistantCase[];
}
