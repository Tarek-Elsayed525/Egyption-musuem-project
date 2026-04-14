using eg_mu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class ConservationLabController : ControllerBase
{
    private readonly GrandEgyptianContext _context;
    public ConservationLabController(GrandEgyptianContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConservationLab>>> GetLabs()
        => await _context.ConservationLabs.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ConservationLab>> GetLabDetails(int id)
    {
        var lab = await _context.ConservationLabs
            .Include(l => l.Artifacts) // جلب القطع الأثرية الموجودة في هذا المعمل
            .FirstOrDefaultAsync(l => l.LabId == id);

        return lab == null ? NotFound() : lab;
    }
}
