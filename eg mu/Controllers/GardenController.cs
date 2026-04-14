using eg_mu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class GardenController : ControllerBase
{
    private readonly GrandEgyptianContext _context;

    public GardenController(GrandEgyptianContext context)
    {
        _context = context;
    }

    // عرض كل الحدائق للجميع
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Garden>>> GetAllGardens()
    {
        return await _context.Gardens.ToListAsync();
    }
}
