using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvocateAssist.Entities;

public partial class LegalAssistant
{
    public int LegalAssistantId { get; set; }

    public string LegalAssistantFname { get; set; } = null!;

    public string LegalAssistantLname { get; set; } = null!;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime JoinDate { get; set; }

    public string Email { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public bool IsActive { get; set; }

    public decimal? MonthlyStipend { get; set; }

    public string NidNumber { get; set; }

    public string? Picture { get; set; } = "noimage.png";
    [NotMapped]
    public IFormFile? PictureFile { get; set; }

    public string? BarLicenseNumber { get; set; }

    public string? Division { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public virtual ICollection<LegalAssistantCase> LegalAssistantCases { get; set; } = new List<LegalAssistantCase>();
}
