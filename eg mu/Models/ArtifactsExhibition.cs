using System;
using System.Collections.Generic;

namespace eg_mu.Models;

public partial class ArtifactsExhibition
{
    public int ArtifactId { get; set; }

    public int ExhibitionId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public virtual Artifact Artifact { get; set; } = null!;

    public virtual Exhibition Exhibition { get; set; } = null!;
}
