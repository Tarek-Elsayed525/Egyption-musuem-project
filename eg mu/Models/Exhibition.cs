using System;
using System.Collections.Generic;

namespace eg_mu.Models;

public partial class Exhibition
{
    public int ExhibitionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int SectionId { get; set; }

    public virtual ICollection<ArtifactsExhibition> ArtifactsExhibitions { get; set; } = new List<ArtifactsExhibition>();

    public virtual MuseumSection Section { get; set; } = null!;
}
