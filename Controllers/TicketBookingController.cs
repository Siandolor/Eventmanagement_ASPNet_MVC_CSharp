using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventmanagement.Models.Tickets;

namespace Eventmanagement.Controllers
{
    public class TicketBookingController : Controller
    {
        private readonly EventmanagementContext _context;

        public TicketBookingController(EventmanagementContext context)
        {
            _context = context;
        }

        // GET: TicketBooking
        public async Task<IActionResult> Index()
        {
            var eventmanagementContext = _context.Tickets.Include(t => t.Event).Include(t => t.SeatUnit).Include(t => t.Session);
            return View(await eventmanagementContext.ToListAsync());
        }

        // GET: TicketBooking/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.SeatUnit)
                .Include(t => t.Session)
                .FirstOrDefaultAsync(m => m.Ticket_Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: TicketBooking/Create
        public IActionResult Create()
        {
            ViewData["Event_Id"] = new SelectList(_context.EventBasics, "Event_Id", "Title");
            ViewData["EventSeatUnit_Id"] = new SelectList(_context.EventSeatUnits, "EventSeatUnit_Id", "EventSeatUnit_Id");
            ViewData["EventSession_Id"] = new SelectList(_context.EventSessions, "EventSession_Id", "Title");
            return View();
        }

        // POST: TicketBooking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ticket_Id,TicketNumber,Event_Id,EventSession_Id,EventSeatUnit_Id,Category,Price,Currency,Status,ReservationExpiresAtUtc,CreatedAtUtc,PaidAtUtc,CancelledAtUtc,CheckedInAtUtc,HolderFirstName,HolderLastName,HolderEmail,HolderPhone,CodePayload,Order_Id,PaymentProvider,PaymentReference,RowVersion")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Ticket_Id = Guid.NewGuid();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Event_Id"] = new SelectList(_context.EventBasics, "Event_Id", "Title", ticket.Event_Id);
            ViewData["EventSeatUnit_Id"] = new SelectList(_context.EventSeatUnits, "EventSeatUnit_Id", "EventSeatUnit_Id", ticket.EventSeatUnit_Id);
            ViewData["EventSession_Id"] = new SelectList(_context.EventSessions, "EventSession_Id", "Title", ticket.EventSession_Id);
            return View(ticket);
        }

        // GET: TicketBooking/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["Event_Id"] = new SelectList(_context.EventBasics, "Event_Id", "Title", ticket.Event_Id);
            ViewData["EventSeatUnit_Id"] = new SelectList(_context.EventSeatUnits, "EventSeatUnit_Id", "EventSeatUnit_Id", ticket.EventSeatUnit_Id);
            ViewData["EventSession_Id"] = new SelectList(_context.EventSessions, "EventSession_Id", "Title", ticket.EventSession_Id);
            return View(ticket);
        }

        // POST: TicketBooking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Ticket_Id,TicketNumber,Event_Id,EventSession_Id,EventSeatUnit_Id,Category,Price,Currency,Status,ReservationExpiresAtUtc,CreatedAtUtc,PaidAtUtc,CancelledAtUtc,CheckedInAtUtc,HolderFirstName,HolderLastName,HolderEmail,HolderPhone,CodePayload,Order_Id,PaymentProvider,PaymentReference,RowVersion")] Ticket ticket)
        {
            if (id != ticket.Ticket_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Ticket_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Event_Id"] = new SelectList(_context.EventBasics, "Event_Id", "Title", ticket.Event_Id);
            ViewData["EventSeatUnit_Id"] = new SelectList(_context.EventSeatUnits, "EventSeatUnit_Id", "EventSeatUnit_Id", ticket.EventSeatUnit_Id);
            ViewData["EventSession_Id"] = new SelectList(_context.EventSessions, "EventSession_Id", "Title", ticket.EventSession_Id);
            return View(ticket);
        }

        // GET: TicketBooking/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.SeatUnit)
                .Include(t => t.Session)
                .FirstOrDefaultAsync(m => m.Ticket_Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: TicketBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(e => e.Ticket_Id == id);
        }
    }
}
