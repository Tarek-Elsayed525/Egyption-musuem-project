using System;
using System.Collections.Generic;

namespace eg_mu.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int VisitorId { get; set; }

    public int PriceId { get; set; }

    public string? Nationality { get; set; }

    public bool? IsStudent { get; set; }

    public string? AgeCategory { get; set; }

    public int? Quantity { get; set; }

    public DateOnly VisitDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? BookingDate { get; set; }

    public virtual TicketPrice Price { get; set; } = null!;

    public virtual Visitor Visitor { get; set; } = null!;
}
