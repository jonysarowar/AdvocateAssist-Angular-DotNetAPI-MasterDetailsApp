using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvocateAssist.Entities;

public partial class LegalAssistantCase
{
    public int LegalAssistantCaseId { get; set; }

    public string CaseTitle { get; set; } = null!;

    public string? Firnumber { get; set; }

    //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime FilingDate { get; set; }
    public int LegalAssistantId { get; set; }
    public int CaseId { get; set; }
    public virtual Case Case { get; set; } = null!;

    public virtual LegalAssistant LegalAssistant { get; set; } = null!;
}
