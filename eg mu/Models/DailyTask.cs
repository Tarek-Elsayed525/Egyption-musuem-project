using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eg_mu.Models;

public partial class DailyTask
{
    public int TaskId { get; set; }


    public string TaskDescription { get; set; } = null!;

    public bool? IsCompleted { get; set; }

    public DateOnly? TaskDate { get; set; }

    public int StaffId { get; set; }

    [JsonIgnore]
    public virtual Staff? Staff { get; set; }


    //public DateTime DateAssigned { get; internal set; }
}
