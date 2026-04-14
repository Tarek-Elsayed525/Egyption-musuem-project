using System.Collections.Generic;

namespace eg_mu.Models;

public partial class TicketPrice
{
    public int PriceId { get; set; }

    public string CategoryName { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public string? TicketType { get; set; }
}
