namespace Eventmanagement.Controllers;

[Authorize(Roles = nameof(UserRole.Location) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Coworker))]
[Authorize(Policy = "LocationOrCoworkerOnly")]

public class LocationDesignerController : Controller
{
    private readonly EventmanagementContext _context;
    private readonly ILogger<LocationDesignerController> _logger;

    public LocationDesignerController(EventmanagementContext context, ILogger<LocationDesignerController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index(Guid? locationId)
    {
        Guid effectiveLocationId;

        if ((User.IsInRole(nameof(UserRole.Admin)) || User.IsInRole(nameof(UserRole.Coworker))) && locationId != null)
        {
            effectiveLocationId = locationId.Value;
        }
        else
        {
            string? rawId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(rawId, out effectiveLocationId))
                return Forbid();
        }

        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Location_Id == effectiveLocationId);
        ViewBag.Location = location;

        var seats = await _context.SeatUnits
            .Where(s => s.Location_Id == effectiveLocationId)
            .Include(s => s.Location)
            .ToListAsync();

        return View(seats);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seatUnit = await _context.SeatUnits
            .Include(s => s.Location)
            .FirstOrDefaultAsync(m => m.SeatUnit_Id == id);
        if (seatUnit == null)
        {
            return NotFound();
        }

        return View(seatUnit);
    }

    public IActionResult Create(Guid? locationId = null)
    {
        Guid? effectiveLocationId = null;

        if ((User.IsInRole(nameof(UserRole.Admin)) || User.IsInRole(nameof(UserRole.Coworker))) && locationId != null)
        {
            effectiveLocationId = locationId;
        }
        else
        {
            string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(currentUserId, out Guid userGuid))
            {
                return Forbid();
            }

            effectiveLocationId = userGuid;
        }

        var userLocation = _context.Locations
            .FirstOrDefault(l => l.Location_Id == effectiveLocationId);

        if (userLocation == null)
        {
            ModelState.AddModelError("", "Keine gültige Location für den aktuellen Benutzer gefunden.");
            ViewBag.Location_Id = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        else
        {
            ViewBag.Location_Id = new SelectList(
                new List<Location> { userLocation },
                "Location_Id",
                "Location_CompanyName"
            );
        }

        ViewBag.BlockType = GetEnumSelectList<BlockIdentifier>();
        ViewBag.Block_Position = GetEnumSelectList<PositionType>();
        ViewBag.Level = GetEnumSelectList<LevelType>();
        ViewBag.Level_Position = GetEnumSelectList<PositionType>();
        ViewBag.Box = GetEnumSelectList<BoxIdentifier>();
        ViewBag.Box_Position = GetEnumSelectList<PositionType>();
        ViewBag.Row_Position = GetEnumSelectList<PositionType>();
        ViewBag.Seat_Position = GetEnumSelectList<PositionType>();

        return View(new SeatUnit
        {
            Location_Id = effectiveLocationId ?? Guid.Empty
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SeatUnit_Id,Location_Id,BlockType,BlockName,Block_Position,Level,Level_Position,Box,Box_Position,Row,Row_Position,SeatNumber,Seat_Position,IsStandingArea,IsSelected,IsReserved,IsBlocked")] SeatUnit seatUnit, Guid? locationId)
    {
        Guid effectiveLocationId;

        if ((User.IsInRole(nameof(UserRole.Admin)) || User.IsInRole(nameof(UserRole.Coworker))) && locationId != null)
        {
            effectiveLocationId = locationId.Value;
        }
        else
        {
            string? rawId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(rawId, out effectiveLocationId))
                return Forbid();
        }

        if (seatUnit.Location_Id == Guid.Empty)
            seatUnit.Location_Id = effectiveLocationId;

        var userLocation = await _context.Locations
            .FirstOrDefaultAsync(l => l.Location_Id == seatUnit.Location_Id);

        if (userLocation == null)
        {
            ModelState.AddModelError("", "Keine gültige Location gefunden.");
            ViewBag.Location_Id = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        else
        {
            ModelState.Remove("Location");

            if (ModelState.IsValid)
            {
                seatUnit.SeatUnit_Id = Guid.NewGuid();
                _context.Add(seatUnit);
                await _context.SaveChangesAsync();

                userLocation.Location_SeatingsAmount = _context.SeatUnits
                    .Count(s => s.Location_Id == seatUnit.Location_Id && !s.IsStandingArea);

                userLocation.Location_StandingsAmount = _context.SeatUnits
                    .Count(s => s.Location_Id == seatUnit.Location_Id && s.IsStandingArea);

                _context.Update(userLocation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { locationId = seatUnit.Location_Id });
            }

            ViewBag.Location_Id = new SelectList(
                new List<Location> { userLocation },
                "Location_Id",
                "Location_CompanyName"
            );
        }

        ViewBag.BlockType = GetEnumSelectList<BlockIdentifier>();
        ViewBag.Block_Position = GetEnumSelectList<PositionType>();
        ViewBag.Level = GetEnumSelectList<LevelType>();
        ViewBag.Level_Position = GetEnumSelectList<PositionType>();
        ViewBag.Box = GetEnumSelectList<BoxIdentifier>();
        ViewBag.Box_Position = GetEnumSelectList<PositionType>();
        ViewBag.Row_Position = GetEnumSelectList<PositionType>();
        ViewBag.Seat_Position = GetEnumSelectList<PositionType>();

        return View(seatUnit);
    }

    public IActionResult BulkCreate(Guid? locationId = null)
    {
        ViewBag.BlockType = GetEnumSelectList<BlockIdentifier>();
        ViewBag.Block_Position = GetEnumSelectList<PositionType>();
        ViewBag.Level = GetEnumSelectList<LevelType>();
        ViewBag.Level_Position = GetEnumSelectList<PositionType>();
        ViewBag.Box = GetEnumSelectList<BoxIdentifier>();
        ViewBag.Box_Position = GetEnumSelectList<PositionType>();
        ViewBag.Row_Position = GetEnumSelectList<PositionType>();
        ViewBag.Seat_Position = GetEnumSelectList<PositionType>();

        return View(new SeatUnitBulkCreator
        {
            Location_Id = locationId ?? Guid.Empty
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkCreate(SeatUnitBulkCreator model)
    {
        if (model.Location_Id == Guid.Empty)
            return Forbid();

        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Location_Id == model.Location_Id);
        if (location == null)
            return Forbid();

        var seatUnits = new List<SeatUnit>();

        for (int row = model.RowStart; row <= model.RowEnd; row++)
        {
            string rowName = $"{model.RowPrefix}{row}";
            for (int seat = model.SeatStart; seat <= model.SeatEnd; seat++)
            {
                seatUnits.Add(new SeatUnit
                {
                    SeatUnit_Id = Guid.NewGuid(),
                    Location_Id = location.Location_Id,
                    BlockType = model.BlockType,
                    BlockName = model.BlockName,
                    Block_Position = model.Block_Position,
                    Level = model.Level,
                    Level_Position = model.Level_Position,
                    Box = model.Box,
                    Box_Position = model.Box_Position,
                    Row = rowName,
                    Row_Position = model.Row_Position,
                    SeatNumber = model.IsStandingArea ? 0 : seat,
                    Seat_Position = model.Seat_Position,
                    IsStandingArea = model.IsStandingArea
                });
            }
        }

        _context.SeatUnits.AddRange(seatUnits);

        location.Location_SeatingsAmount = _context.SeatUnits
            .Count(s => s.Location_Id == location.Location_Id && !s.IsStandingArea);

        location.Location_StandingsAmount = _context.SeatUnits
            .Count(s => s.Location_Id == location.Location_Id && s.IsStandingArea);

        _context.Update(location);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { locationId = location.Location_Id });
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seatUnit = await _context.SeatUnits.FindAsync(id);
        if (seatUnit == null)
        {
            return NotFound();
        }

        ViewBag.BlockType = GetEnumSelectList<BlockIdentifier>();
        ViewBag.Block_Position = GetEnumSelectList<PositionType>();
        ViewBag.Level = GetEnumSelectList<LevelType>();
        ViewBag.Level_Position = GetEnumSelectList<PositionType>();
        ViewBag.Box = GetEnumSelectList<BoxIdentifier>();
        ViewBag.Box_Position = GetEnumSelectList<PositionType>();
        ViewBag.Row_Position = GetEnumSelectList<PositionType>();
        ViewBag.Seat_Position = GetEnumSelectList<PositionType>();

        return View(seatUnit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("SeatUnit_Id,Location_Id,BlockType,BlockName,Block_Position,Level,Level_Position,Box,Box_Position,Row,Row_Position,SeatNumber,Seat_Position,IsStandingArea,IsSelected,IsReserved,IsBlocked")] SeatUnit seatUnit, Guid? locationId)
    {
        if (id != seatUnit.SeatUnit_Id)
            return NotFound();

        if (seatUnit.Location_Id == Guid.Empty)
        {
            if ((User.IsInRole(nameof(UserRole.Admin)) || User.IsInRole(nameof(UserRole.Coworker))) && locationId.HasValue)
            {
                seatUnit.Location_Id = locationId.Value;
            }
            else
            {
                string? rawId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(rawId, out Guid fallbackId))
                    return Forbid();
                seatUnit.Location_Id = fallbackId;
            }
        }

        ModelState.Remove("Location");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(seatUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { locationId = seatUnit.Location_Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatUnitExists(seatUnit.SeatUnit_Id))
                    return NotFound();
                throw;
            }
        }

        ViewBag.BlockType = GetEnumSelectList<BlockIdentifier>();
        ViewBag.Block_Position = GetEnumSelectList<PositionType>();
        ViewBag.Level = GetEnumSelectList<LevelType>();
        ViewBag.Level_Position = GetEnumSelectList<PositionType>();
        ViewBag.Box = GetEnumSelectList<BoxIdentifier>();
        ViewBag.Box_Position = GetEnumSelectList<PositionType>();
        ViewBag.Row_Position = GetEnumSelectList<PositionType>();
        ViewBag.Seat_Position = GetEnumSelectList<PositionType>();

        return View(seatUnit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var seatUnit = await _context.SeatUnits
            .Include(s => s.Location)
            .FirstOrDefaultAsync(s => s.SeatUnit_Id == id);

        if (seatUnit == null)
            return NotFound();

        Guid locationId = seatUnit.Location_Id;

        _context.SeatUnits.Remove(seatUnit);
        await _context.SaveChangesAsync();

        var location = await _context.Locations.FindAsync(locationId);
        if (location != null)
        {
            location.Location_SeatingsAmount = _context.SeatUnits
                .Count(s => s.Location_Id == locationId && !s.IsStandingArea);

            location.Location_StandingsAmount = _context.SeatUnits
                .Count(s => s.Location_Id == locationId && s.IsStandingArea);

            _context.Update(location);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new { locationId });
    }

    private bool SeatUnitExists(Guid id)
    {
        return _context.SeatUnits.Any(e => e.SeatUnit_Id == id);
    }

    private List<SelectListItem> GetEnumSelectList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new SelectListItem
            {
                Value = Convert.ToInt32(e).ToString(),
                Text = e.GetType()
                        .GetField(e.ToString())?
                        .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();
    }
}
