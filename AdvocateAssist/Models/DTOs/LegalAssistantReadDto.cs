using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvocateAssist.Models.DTOs
{
    public class LegalAssistantReadDto
    {
        public int LegalAssistantId { get; set; }
        public string LegalAssistantFname { get; set; }
        public string LegalAssistantLname { get; set; }
        public DateTime JoinDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public decimal? MonthlyStipend { get; set; }
        public string NidNumber { get; set; }
        public string? Picture { get; set; }
        public string? BarLicenseNumber { get; set; }

        public List<LegalAssistantCaseReadDto> LegalAssistantCases { get; set; } = new();
    }

    public class LegalAssistantCaseReadDto
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public string CaseTitle { get; set; }
        public string? Firnumber { get; set; }
        public DateTime FilingDate { get; set; }
    }

    public class LegalAssistantCreateUpdateDto
    {
        public int LegalAssistantId { get; set; }
        public string LegalAssistantFname { get; set; }
        public string LegalAssistantLname { get; set; }
        public DateTime JoinDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public decimal? MonthlyStipend { get; set; }
        public string NidNumber { get; set; }
        public IFormFile? PictureFile { get; set; }
        public string? Picture { get; set; }
        public string? BarLicenseNumber { get; set; }

        public string LegalAssistantCasesJson { get; set; }


        //[NotMapped]
        //public List<LegalAssistantCaseDto>? ParsedLegalAssistantCases =>
        //!string.IsNullOrEmpty(LegalAssistantCasesJson)
        //    ? JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(LegalAssistantCasesJson)
        //    : new List<LegalAssistantCaseDto>();
    }

    public class LegalAssistantCaseDto
    {
        public int CaseId { get; set; }
        public string CaseTitle { get; set; }
        public string? Firnumber { get; set; }
        public DateTime FilingDate { get; set; }
    }

    public class CaseReadDto
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; } = null!;
    }





}
