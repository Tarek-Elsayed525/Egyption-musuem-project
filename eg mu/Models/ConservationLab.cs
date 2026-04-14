using System;
using System.Collections.Generic;

namespace eg_mu.Models;

public partial class ConservationLab
{
    public int LabId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int SectionId { get; set; }

    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();

    public virtual MuseumSection Section { get; set; } = null!;
}
