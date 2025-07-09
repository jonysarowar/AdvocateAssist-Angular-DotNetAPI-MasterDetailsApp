export interface LegalAssistantCreateUpdate {
  legalAssistantId?: number;
  legalAssistantFname: string;
  legalAssistantLname: string;
  joinDate: Date;
  email: string;
  mobileNo: string;
  isActive: boolean;
  monthlyStipend?: number;
  // nidNumber: number;
  nidNumber: string;
  picture?: string | null;
  pictureFile?: File | null;
  barLicenseNumber?: string;
  legalAssistantCasesJson: string;
}
