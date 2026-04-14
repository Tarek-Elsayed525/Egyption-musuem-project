using System;
using System.Collections.Generic;

namespace eg_mu.Models;
using System.Text.Json.Serialization;



public partial class Attendance
{
    public int AttendanceId { get; set; }
    public int StaffId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    // إضافة [JsonIgnore] هنا هي السر! 
    // بتخلي الـ Swagger ميرغمكش تبعت كائن الموظف كامل
    [JsonIgnore]
    public virtual Staff? Staff { get; set; }
}
