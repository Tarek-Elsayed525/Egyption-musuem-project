using System.Collections.Generic;
using System.Text.Json.Serialization; // ضفنا دي

namespace eg_mu.Models;

public partial class Artifact
{
    public int ArtifactId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Era { get; set; }
    public string? Material { get; set; }
    public int SectionId { get; set; }
    public string? ImageUrl { get; set; }
    public int? LabId { get; set; }

    [JsonIgnore] // بنحطها هنا عشان ميعملش Loop في الـ JSON
    public virtual ICollection<ArtifactsExhibition> ArtifactsExhibitions { get; set; } = new List<ArtifactsExhibition>();

    [JsonIgnore]
    public virtual ConservationLab? Lab { get; set; }

    // هنا مسموح يظهر بيانات القاعة مع القطعة
    public virtual MuseumSection Section { get; set; } = null!;
}