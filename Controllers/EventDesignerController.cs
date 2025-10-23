namespace Eventmanagement.Controllers
{
    public class EventDesignerController : Controller
    {
        private readonly EventmanagementContext _context;

        public EventDesignerController(EventmanagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid? organizerId = null)
        {
            Guid effectiveOrganizerId;

            if ((User.IsInRole(nameof(UserRole.Admin)) || User.IsInRole(nameof(UserRole.Coworker))) && organizerId.HasValue)
            {
                effectiveOrganizerId = organizerId.Value;
            }
            else
            {
                string? rawId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(rawId, out effectiveOrganizerId))
                    return Forbid();
            }

            var organizer = await _context.Organizers.FirstOrDefaultAsync(o => o.Organizer_Id == effectiveOrganizerId);
            if (organizer == null)
                return NotFound();

            ViewBag.Organizer = organizer;
            ViewBag.OrganizerName = organizer.Organizer_CompanyName;

            var events = await _context.EventBasics
                .Where(e => e.Organizer_Id == effectiveOrganizerId)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .ToListAsync();

            var sessions = await _context.EventSessions
                .Include(s => s.Event)
                .ToListAsync();

            var sessionPresenceMap = sessions
                .GroupBy(s => s.Event_Id)
                .ToDictionary(g => g.Key, g => g.Any());

            var sessionMap = sessions
                .GroupBy(s => s.Event_Id)
                .ToDictionary(g => g.Key, g => g.First());

            var attachments = await _context.EventAttachments.ToListAsync();

            var attachmentMap = attachments
                .GroupBy(a => a.Event_Id)
                .ToDictionary(g => g.Key, g => g.Any());

            ViewBag.HasSession = sessionPresenceMap;
            ViewBag.SessionMap = sessionMap;
            ViewBag.HasAttachment = attachmentMap;

            return View(events);
        }

        public IActionResult Create(Guid? organizerId = null)
        {
            Guid? effectiveOrganizerId;

            if ((User.IsInRole("Admin") || User.IsInRole("Coworker")) && organizerId.HasValue)
            {
                effectiveOrganizerId = organizerId;
            }
            else
            {
                var rawId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(rawId, out Guid fallbackId))
                    return Forbid();
                effectiveOrganizerId = fallbackId;
            }

            var organizer = _context.Organizers.FirstOrDefault(o => o.Organizer_Id == effectiveOrganizerId);
            if (organizer == null)
                return NotFound();

            ViewBag.Organizer = organizer;
            ViewData["Location_Id"] = new SelectList(_context.Locations, "Location_Id", "Location_CompanyName");
            ViewBag.Category = GetEnumSelectList<EventCategory>();
            ViewBag.Type = GetEnumSelectList<EventType>();

            return View(new EventBasics
            {
                Organizer_Id = organizer.Organizer_Id,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventBasics eventBasics, Guid? organizerId)
        {
            Guid effectiveOrganizerId;

            if ((User.IsInRole("Admin") || User.IsInRole("Coworker")) && organizerId.HasValue)
            {
                effectiveOrganizerId = organizerId.Value;
            }
            else
            {
                string? rawId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(rawId, out effectiveOrganizerId))
                    return Forbid();
            }

            eventBasics.Organizer_Id = effectiveOrganizerId;

            ModelState.Remove("EventSeatUnits");
            ModelState.Remove("Organizer");
            ModelState.Remove("Location");

            if (ModelState.IsValid)
            {
                eventBasics.Event_Id = Guid.NewGuid();
                _context.Add(eventBasics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { organizerId = effectiveOrganizerId });
            }

            ViewBag.Organizer = await _context.Organizers.FindAsync(effectiveOrganizerId);
            ViewData["Location_Id"] = new SelectList(_context.Locations, "Location_Id", "Location_CompanyName", eventBasics.Location_Id);
            ViewBag.Category = GetEnumSelectList<EventCategory>();
            ViewBag.Type = GetEnumSelectList<EventType>();

            return View(eventBasics);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var eventBasics = await _context.EventBasics.FindAsync(id);
            if (eventBasics == null)
                return NotFound();

            ViewData["Location_Id"] = new SelectList(_context.Locations, "Location_Id", "Location_CompanyName", eventBasics.Location_Id);
            ViewData["Organizer_Id"] = new SelectList(_context.Organizers, "Organizer_Id", "Organizer_CompanyName", eventBasics.Organizer_Id);
            ViewBag.Category = GetEnumSelectList<EventCategory>();
            ViewBag.Type = GetEnumSelectList<EventType>();

            return View(eventBasics);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventBasics eventBasics)
        {
            if (id != eventBasics.Event_Id)
                return NotFound();

            ModelState.Remove("EventSeatUnits");
            ModelState.Remove("Organizer");
            ModelState.Remove("Location");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventBasics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventBasicsExists(eventBasics.Event_Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index), new { organizerId = eventBasics.Organizer_Id });
            }

            ViewData["Location_Id"] = new SelectList(_context.Locations, "Location_Id", "Location_CompanyName", eventBasics.Location_Id);
            ViewData["Organizer_Id"] = new SelectList(_context.Organizers, "Organizer_Id", "Organizer_CompanyName", eventBasics.Organizer_Id);
            ViewBag.Category = GetEnumSelectList<EventCategory>();
            ViewBag.Type = GetEnumSelectList<EventType>();

            return View(eventBasics);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.EventBasics
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Event_Id == id);

            if (eventItem == null)
                return NotFound();

            var seatUnits = await _context.EventSeatUnits
                .Where(s => s.Event_Id == eventItem.Event_Id)
                .Include(s => s.SeatUnit)
                .ToListAsync();

            var sessions = await _context.EventSessions
                .Where(s => s.Event_Id == eventItem.Event_Id)
                .ToListAsync();

            var attachments = await _context.EventAttachments
                .Where(a => a.Event_Id == eventItem.Event_Id)
                .ToListAsync();

            ViewBag.SeatUnits = seatUnits;
            ViewBag.Sessions = sessions;
            ViewBag.Attachments = attachments;

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var eventBasics = await _context.EventBasics.FindAsync(id);
            if (eventBasics != null)
                _context.EventBasics.Remove(eventBasics);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { organizerId = eventBasics?.Organizer_Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(Guid id)
        {
            var eventItem = await _context.EventBasics.FindAsync(id);
            if (eventItem == null) return NotFound();

            eventItem.IsPublished = !eventItem.IsPublished;
            _context.Update(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { organizerId = eventItem.Organizer_Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTicketSale(Guid id)
        {
            var eventItem = await _context.EventBasics.FindAsync(id);
            if (eventItem == null) return NotFound();

            eventItem.TicketSaleOpen = !eventItem.TicketSaleOpen;
            _context.Update(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { organizerId = eventItem.Organizer_Id });
        }

        private bool EventBasicsExists(Guid id)
        {
            return _context.EventBasics.Any(e => e.Event_Id == id);
        }

        private List<SelectListItem> GetEnumSelectList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToInt32(e).ToString(),
                    Text = e.GetType().GetField(e.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                })
                .ToList();
        }

        public IActionResult AddSession(Guid eventId)
        {
            var newSession = new EventSession
            {
                Event_Id = eventId,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today)
            };

            return View(newSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSession(EventSession session)
        {
            ModelState.Remove("Event");

            if (ModelState.IsValid)
            {
                session.EventSession_Id = Guid.NewGuid();
                _context.Add(session);
                await _context.SaveChangesAsync();

                var organizerId = await _context.EventBasics
                    .Where(e => e.Event_Id == session.Event_Id)
                    .Select(e => e.Organizer_Id)
                    .FirstOrDefaultAsync();

                return RedirectToAction("Index", new { organizerId });
            }

            return View(session);
        }

        public async Task<IActionResult> EditSession(Guid sessionId)
        {
            var session = await _context.EventSessions
                .Include(s => s.Event)
                    .ThenInclude(e => e.Location)
                .Include(s => s.Event)
                    .ThenInclude(e => e.Organizer)
                .FirstOrDefaultAsync(s => s.EventSession_Id == sessionId);

            if (session == null)
                return NotFound();

            var sessions = await _context.EventSessions
                .Where(s => s.Event_Id == session.Event_Id)
                .Select(s => new SelectListItem
                {
                    Value = s.EventSession_Id.ToString(),
                    Text = s.Title
                })
                .ToListAsync();

            ViewBag.Sessions = sessions;
            ViewBag.OrganizerId = session.Event.Organizer_Id;
            ViewBag.OrganizerName = session.Event.Organizer.Organizer_CompanyName;
            ViewBag.LocationName = session.Event.Location.Location_CompanyName;

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSession(EventSession session)
        {
            ModelState.Remove("Event");

            if (!ModelState.IsValid)
            {
                ViewBag.Sessions = await _context.EventSessions
                    .Where(s => s.Event_Id == session.Event_Id)
                    .Select(s => new SelectListItem
                    {
                        Value = s.EventSession_Id.ToString(),
                        Text = s.Title
                    })
                    .ToListAsync();

                var evt = await _context.EventBasics
                    .Include(e => e.Organizer)
                    .Include(e => e.Location)
                    .FirstOrDefaultAsync(e => e.Event_Id == session.Event_Id);

                ViewBag.OrganizerId = evt?.Organizer_Id;
                ViewBag.LocationName = evt?.Location?.Location_CompanyName;

                return View(session);
            }

            try
            {
                _context.Update(session);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EventSessions.Any(s => s.EventSession_Id == session.EventSession_Id))
                    return NotFound();
                throw;
            }

            var organizerId = await _context.EventBasics
                .Where(e => e.Event_Id == session.Event_Id)
                .Select(e => e.Organizer_Id)
                .FirstOrDefaultAsync();

            return RedirectToAction(nameof(Index), new { organizerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSession(Guid sessionId)
        {
            var session = await _context.EventSessions.FindAsync(sessionId);
            if (session == null)
                return NotFound();

            var organizerId = await _context.EventBasics
                .Where(e => e.Event_Id == session.Event_Id)
                .Select(e => e.Organizer_Id)
                .FirstOrDefaultAsync();

            _context.EventSessions.Remove(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { organizerId });
        }

        public IActionResult AddAttachment(Guid eventId)
        {
            return View(new EventAttachment { Event_Id = eventId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAttachment(EventAttachment attachment, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                attachment.FileContent = ms.ToArray();
                attachment.FileName = file.FileName;
                attachment.MimeType = file.ContentType;
            }

            attachment.EventAttachment_Id = Guid.NewGuid();
            _context.Add(attachment);
            await _context.SaveChangesAsync();

            var organizerId = await _context.EventBasics
                .Where(e => e.Event_Id == attachment.Event_Id)
                .Select(e => e.Organizer_Id)
                .FirstOrDefaultAsync();

            return RedirectToAction("Index", new { organizerId });
        }

        public async Task<IActionResult> EditAttachment(Guid eventId)
        {
            var attachment = await _context.EventAttachments
                .FirstOrDefaultAsync(a => a.Event_Id == eventId);

            if (attachment == null)
                return NotFound();

            return View(attachment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAttachment(EventAttachment attachment, IFormFile? file)
        {
            var existing = await _context.EventAttachments
                .FirstOrDefaultAsync(a => a.EventAttachment_Id == attachment.EventAttachment_Id);

            if (existing == null)
                return NotFound();

            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                existing.FileContent = ms.ToArray();
                existing.FileName = file.FileName;
                existing.MimeType = file.ContentType;
            }

            _context.Update(existing);
            await _context.SaveChangesAsync();

            var organizerId = await _context.EventBasics
                .Where(e => e.Event_Id == existing.Event_Id)
                .Select(e => e.Organizer_Id)
                .FirstOrDefaultAsync();

            return RedirectToAction(nameof(Index), new { organizerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(Guid attachmentId)
        {
            var attachment = await _context.EventAttachments.FindAsync(attachmentId);
            if (attachment == null)
                return NotFound();

            var organizerId = await _context.EventBasics
                .Where(e => e.Event_Id == attachment.Event_Id)
                .Select(e => e.Organizer_Id)
                .FirstOrDefaultAsync();

            _context.EventAttachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { organizerId });
        }

        public IActionResult EditSeat(Guid eventId)
        {
            var eventEntity = _context.EventBasics
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .FirstOrDefault(e => e.Event_Id == eventId);

            if (eventEntity == null) return NotFound();

            var locationId = eventEntity.Location_Id;

            var seatUnits = _context.SeatUnits
                .Where(s => s.Location_Id == locationId)
                .ToList();

            var inputModel = new SeatPricingInputModel
            {
                Event_Id = eventId,
                BasePrice = 0m,
                StandingDiscount = 0m,
                BlockSurcharges = new(),
                LevelSurcharges = new(),
                BoxSurcharges = new(),
                RowSurcharges = new()
            };

            ViewBag.UsedBlocks = seatUnits
                .Where(s => !string.IsNullOrWhiteSpace(s.BlockName) && s.BlockType != BlockIdentifier.Unspecified)
                .GroupBy(s => s.BlockName!)
                .ToDictionary(g => g.Key, g => g.First().BlockType);

            ViewBag.UsedLevels = seatUnits
                .Where(s => s.Level != LevelType.Unspecified)
                .GroupBy(s => s.Level.ToString())
                .ToDictionary(g => g.Key, g => g.First().Level);

            ViewBag.UsedBoxes = seatUnits
                .Where(s => s.Box != BoxIdentifier.Unspecified)
                .GroupBy(s => s.Box.ToString())
                .ToDictionary(g => g.Key, g => g.First().Box);

            ViewBag.UsedRows = seatUnits
                .Where(s => !string.IsNullOrWhiteSpace(s.Row))
                .Select(s => s.Row!)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            ViewBag.OrganizerId = eventEntity.Organizer_Id;
            ViewBag.OrganizerName = eventEntity.Organizer.Organizer_CompanyName;
            ViewBag.LocationName = eventEntity.Location.Location_CompanyName;

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSeat(SeatPricingInputModel input)
        {
            var eventEntity = await _context.EventBasics
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Event_Id == input.Event_Id);

            if (eventEntity == null) return NotFound();

            var locationId = eventEntity.Location_Id;

            var seatUnits = await _context.SeatUnits
                .Where(s => s.Location_Id == locationId)
                .ToListAsync();

            var disabledBlocks = Request.Form["DisabledBlocks"].ToHashSet();
            var disabledLevels = Request.Form["DisabledLevels"].ToHashSet();
            var disabledBoxes = Request.Form["DisabledBoxes"].ToHashSet();
            var disabledRows = Request.Form["DisabledRows"].ToHashSet();

            var calculator = new SeatPriceCalculator(
                input.BasePrice,
                input.StandingDiscount,
                input.BlockSurcharges,
                input.LevelSurcharges,
                input.BoxSurcharges,
                input.RowSurcharges
            );

            var existingEventSeats = await _context.EventSeatUnits
                .Where(e => e.Event_Id == input.Event_Id)
                .ToListAsync();

            _context.EventSeatUnits.RemoveRange(existingEventSeats);
            await _context.SaveChangesAsync();

            foreach (var seatUnit in seatUnits)
            {
                if (
                    (!string.IsNullOrWhiteSpace(seatUnit.BlockName) && disabledBlocks.Contains(seatUnit.BlockName)) ||
                    (seatUnit.Level != LevelType.Unspecified && disabledLevels.Contains(seatUnit.Level.ToString())) ||
                    (seatUnit.Box != BoxIdentifier.Unspecified && disabledBoxes.Contains(seatUnit.Box.ToString())) ||
                    (!string.IsNullOrWhiteSpace(seatUnit.Row) && disabledRows.Contains(seatUnit.Row))
                )
                {
                    continue;
                }

                var price = calculator.CalculatePrice(seatUnit);

                var eventSeat = new EventSeatUnit
                {
                    EventSeatUnit_Id = Guid.NewGuid(),
                    Event_Id = input.Event_Id,
                    SeatUnit_Id = seatUnit.SeatUnit_Id,
                    TicketPrice = price,
                    Category = input.Category,
                    Status = input.Status
                };

                _context.EventSeatUnits.Add(eventSeat);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { organizerId = eventEntity.Organizer_Id });
        }
    }
}
