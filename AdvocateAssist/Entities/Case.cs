using System;
using System.Collections.Generic;

namespace AdvocateAssist.Entities;

public partial class Case
{
    public int CaseId { get; set; }

    public string CaseNumber { get; set; } = null!;

    public virtual ICollection<LegalAssistantCase> LegalAssistantCases { get; set; } = new List<LegalAssistantCase>();
}
