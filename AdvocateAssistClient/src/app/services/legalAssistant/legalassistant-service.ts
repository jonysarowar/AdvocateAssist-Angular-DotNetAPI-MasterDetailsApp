import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Case } from '../../models/legalAssistant/case';
import { LegalAssistantCreateUpdate } from '../../models/legalAssistant/legal-assistant-create-update';
import { LegalAssistant } from '../../models/legalAssistant/legalassistant';

@Injectable({
  providedIn: 'root',
})
export class LegalassistantService {
  private apiUrl = 'http://localhost:5268/api/LegalAssistants';
  private casesApiUrl = 'http://localhost:5268/api/Cases';

  constructor(private http: HttpClient) {}

  getLegalAssistants(): Observable<LegalAssistant[]> {
    return this.http.get<LegalAssistant[]>(this.apiUrl);
  }

  getLegalAssistant(id: number): Observable<LegalAssistant> {
    return this.http.get<LegalAssistant>(`${this.apiUrl}/${id}`);
  }

  getCases(): Observable<Case[]> {
    return this.http.get<Case[]>(this.casesApiUrl);
  }

  private _buildFormData(
    legalAssistant: LegalAssistantCreateUpdate,
    legalAssistantId?: number
  ): FormData {
    const formData = new FormData();

    if (legalAssistantId !== undefined) {
      formData.append('legalAssistantId', legalAssistantId.toString());
    }

    formData.append('legalAssistantFname', legalAssistant.legalAssistantFname);
    formData.append('legalAssistantLname', legalAssistant.legalAssistantLname);
    formData.append('joinDate', legalAssistant.joinDate.toISOString());
    formData.append('email', legalAssistant.email);
    formData.append('mobileNo', legalAssistant.mobileNo);
    formData.append('isActive', legalAssistant.isActive.toString());
    // formData.append('nidNumber', legalAssistant.nidNumber.toString());
    formData.append('nidNumber', legalAssistant.nidNumber);

    if (
      legalAssistant.monthlyStipend !== undefined &&
      legalAssistant.monthlyStipend !== null
    ) {
      formData.append(
        'monthlyStipend',
        legalAssistant.monthlyStipend.toString()
      );
    }

    if (legalAssistant.barLicenseNumber) {
      formData.append('barLicenseNumber', legalAssistant.barLicenseNumber);
    }

    if (legalAssistant.pictureFile) {
      formData.append(
        'pictureFile',
        legalAssistant.pictureFile,
        legalAssistant.pictureFile.name
      );
    } else if (legalAssistant.picture && legalAssistantId !== undefined) {
      formData.append('picture', legalAssistant.picture);
    }
    formData.append(
      'legalAssistantCasesJson',
      legalAssistant.legalAssistantCasesJson
    );
    return formData;
  }

  createLegalAssistant(
    legalAssistant: LegalAssistantCreateUpdate
  ): Observable<LegalAssistant> {
    const formData = this._buildFormData(legalAssistant);
    return this.http.post<LegalAssistant>(this.apiUrl, formData);
  }

  UpdateLegalAssistant(
    id: number,
    legalAssistant: LegalAssistantCreateUpdate
  ): Observable<any> {
    const formData = this._buildFormData(legalAssistant, id);
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteLegalAssistant(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
