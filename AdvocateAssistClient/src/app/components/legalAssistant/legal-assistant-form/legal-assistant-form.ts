import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Case } from '../../../models/legalAssistant/case';
import { LegalAssistantCreateUpdate } from '../../../models/legalAssistant/legal-assistant-create-update';
import { LegalAssistant } from '../../../models/legalAssistant/legalassistant';
import { LegalassistantService } from '../../../services/legalAssistant/legalassistant-service';

@Component({
  selector: 'app-legal-assistant-form',
  imports: [CommonModule,FormsModule,ReactiveFormsModule,RouterLink],
  templateUrl: './legal-assistant-form.html',
  styleUrl: './legal-assistant-form.css',
})
export class LegalAssistantForm implements OnInit {
  assistantForm!: FormGroup;
  isEditMode = false;
  legalAssistantId: number | null = null;
  cases: Case[] = [];
  selectedFile: File | null = null;
  currentPicture: string | null = null;
  imageUrlBase = 'http://localhost:5268/images/';
  imagePreviewUrl: string | ArrayBuffer | null = null; 


  constructor( private fb: FormBuilder,
    private service: LegalassistantService,
    private route: ActivatedRoute,
    private router: Router
  ) { }


  ngOnInit(): void {
    this.loadCases();
    this.initForm();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.legalAssistantId = +id;
        this.loadAssistant(this.legalAssistantId);
      }
    });
  }

  initForm(): void {
    this.assistantForm = this.fb.group({
      legalAssistantFname: ['',[Validators.required]],
      legalAssistantLname: ['',[Validators.required]],
      email: ['', [Validators.email]],
      mobileNo: ['',[Validators.required]],
      joinDate: ['',[Validators.required]],
      isActive: [true],
      // nidNumber: [null],
      nidNumber: ['', [Validators.required]],
      barLicenseNumber: [null],
      monthlyStipend: [null],
      pictureFile: [null],
      legalAssistantCases:this.fb.array([])
    });
  }


  loadCases():void {
      this.service.getCases().subscribe({
        next: (data: Case[]) => {
          this.cases = data;
        },
        error: (err) => {
          console.error('Error loading case:', err);
          alert('Could not load case. Please try again later.');
        }
      });
  }

  loadAssistant(id: number): void {
    this.service.getLegalAssistant(id).subscribe({
      next: (la: LegalAssistant) => {
        this.assistantForm.patchValue({
          legalAssistantFname: la.legalAssistantFname,
          legalAssistantLname: la.legalAssistantLname,
          email: la.email,
          mobileNo: la.mobileNo,
          joinDate: la.joinDate ? this.formatDateToInput(la.joinDate) : '',
          isActive: la.isActive,
          monthlyStipend: la.monthlyStipend,
          // nidNumber: la.nidNumber ?? 0, 
          nidNumber: la.nidNumber ?? '',
          barLicenseNumber: la.barLicenseNumber ?? '' 
        });
        this.currentPicture = la.picture ?? null;       
        if (this.currentPicture) {
          this.imagePreviewUrl = this.imageUrlBase + this.currentPicture;
        } else {
          this.imagePreviewUrl = null; 
        }
        this.legalAssistantCases.clear();

        la.legalAssistantCases.forEach(cs => {
          this.addLegalAssistantCase(
            cs.caseId, 
            cs.caseTitle,
            cs.firnumber??'',
            // cs.filingDate);
            // cs.filingDate ? new Date(cs.filingDate).toISOString().split('T')[0] : ''
            cs.filingDate ? this.formatDateToInput(cs.filingDate) : ''
          );
        });
      },
      error: (err) => {
        console.error('Error loading assistant for edit:', err);
        alert('Failed to load assistant. Redirecting to list.');
        this.router.navigate(['/legalassistants']);
      }
    });
  }

  //এডিট করলে ডেট একদিন করে কমে তাই এই ফরম্যাট করা হয়েছে
  formatDateToInput(date: string | Date): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = ('0' + (d.getMonth() + 1)).slice(-2);
    const day = ('0' + d.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }


  get legalAssistantCases(): FormArray {
      return this.assistantForm.get('legalAssistantCases') as FormArray;
  }

  // নতুন কেস তৈরি করার মেথড
  newLegalAssistantCase(
    caseId: number = 0, 
    caseTitle: string = '', 
    firnumber: string = '', 
    filingDate: string | Date = ''
    // filingDate: Date = new Date()
  ): FormGroup {
    return this.fb.group({
      caseId: [caseId, Validators.required],
      caseTitle: [caseTitle, Validators.required],
      firnumber: [firnumber],
      filingDate: [filingDate, Validators.required]
    });
  }

  // কেস যোগ করার মেথড
  addLegalAssistantCase(
    caseId?: number, 
    caseTitle?: string, 
    firnumber?: string, 
    filingDate: string | Date = ''
    // filingDate?: Date
  ): void {
    this.legalAssistantCases.push(
      this.newLegalAssistantCase(caseId, caseTitle, firnumber, filingDate)
    );
  }



  removeLegalAssistantCase(index: number): void {
      this.legalAssistantCases.removeAt(index);
  }


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      const reader = new FileReader(); 
      reader.onload = () => {
        this.imagePreviewUrl = reader.result;
      };
      reader.readAsDataURL(this.selectedFile); 
    } else {
      this.selectedFile = null; 
      this.imagePreviewUrl = null; 
    }
  }


  onSubmit(): void {
      this.assistantForm.markAllAsTouched();
      this.markFormArrayControlsAsTouched(this.legalAssistantCases);

      if (this.assistantForm.invalid) {
        alert('Please fill in all required fields.');
        return;
      }
      const formValues = this.assistantForm.value;
      let joinDate: Date;
      if (formValues.joinDate) {
        joinDate = new Date(formValues.joinDate);
        if (isNaN(joinDate.getTime())) {       
          alert('The Join Date you entered is not valid.');
          return;
        }
      } else 
      { 
        alert('The Join Date you entered is not valid.');return;
      }
      
      const legalAssistantCasesData = formValues.legalAssistantCases.map((cs: any) => ({
        caseId: Number(cs.caseId),
        caseTitle: String(cs.caseTitle),
        firnumber:String(cs.firnumber),
        filingDate: new Date(cs.filingDate)
      }));


      const assistantData: LegalAssistantCreateUpdate = {
        legalAssistantFname: formValues.legalAssistantFname,
        legalAssistantLname: formValues.legalAssistantLname,
        joinDate: joinDate,
        email:formValues.email,
        mobileNo: formValues.mobileNo,
        isActive:formValues.isActive,
        monthlyStipend:formValues.monthlyStipend,
        // Number(formValues.nidNumber),
        nidNumber: formValues.nidNumber,
        pictureFile: this.selectedFile,
        barLicenseNumber:formValues.barLicenseNumber,
        legalAssistantCasesJson: JSON.stringify(legalAssistantCasesData)
      };
      if (this.isEditMode && this.legalAssistantId !== null) {
        if (!this.selectedFile && this.currentPicture) {
          assistantData.picture = this.currentPicture;
        }
        this.service.UpdateLegalAssistant(this.legalAssistantId, assistantData).subscribe({
          next: () => {
            alert('Assistant updated successfully!');
            this.router.navigate(['/legalassistants']);
          },
          error: (err) => {        
            alert('Failed to update Assistant');
          }
        });
      } else {
        this.service.createLegalAssistant(assistantData).subscribe({
          next: () => {
            alert('Assistant created successfully!');
            this.router.navigate(['/legalassistants']);
          },
          error: (err) => {
            alert('Failed to create Assistant.');
          }
        });
      }
  }
  
  private markFormArrayControlsAsTouched(formArray: FormArray): void {
    formArray.controls.forEach(control => {
      if (control instanceof FormGroup) {
        Object.values(control.controls).forEach(innerControl => innerControl.markAsTouched());
      } else { control.markAsTouched(); }
    });
  }

  
  

}
