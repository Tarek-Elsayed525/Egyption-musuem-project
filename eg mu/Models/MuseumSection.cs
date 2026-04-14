using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eg_mu.Models;

public partial class MuseumSection
{
    public int SectionId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();

    public virtual ICollection<ConservationLab> ConservationLabs { get; set; } = new List<ConservationLab>();

    public virtual ICollection<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();

    public virtual ICollection<Garden> Gardens { get; set; } = new List<Garden>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
  
}
