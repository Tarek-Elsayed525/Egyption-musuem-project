using System;
using System.Collections.Generic;

namespace eg_mu.Models;

public partial class Garden
{
    public int GardenId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int? SectionId { get; set; }

    public virtual MuseumSection? Section { get; set; }
}
