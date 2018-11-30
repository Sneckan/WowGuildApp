using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WowGuildApp.Data;
using WowGuildApp.Models;
using WowGuildApp.ViewModels;

namespace WowGuildApp
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private UserManager<User> userManager;
        private Task<User> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public EventsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;

        }

        [Authorize]
        public IActionResult Index()
        {
            List<Event> model;

            model = _context.Events.ToList();

            return View(model);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            var @host = await _context.Users.FirstOrDefaultAsync(u => u.Id == @event.hostId);

            var @signups = _context.Signups.Where(s => s.EventId == @event.Id).ToList();

            foreach (Signup s in signups)
            {
                s.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == s.UserId);
            }

            EventDetailViewModel model = new EventDetailViewModel
            {
                Event = @event,
                Host = @host,
                Signups = @signups,
            };


            return View(model);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet,Route("Events/Create/{date}")]
        public IActionResult Create(string date)
        {
            Event model = new Event
            {
                Date = DateTime.Parse(date)
            };
            return View(model);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,InviteTime,StartTime,LastSignup,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                @event.host = user;
                


                TimeSpan ts = @event.InviteTime.TimeOfDay;
                @event.InviteTime = @event.Date.Date + ts;

                ts = @event.StartTime.TimeOfDay;
                @event.StartTime = @event.Date.Date + ts;

                ts = @event.LastSignup.TimeOfDay;
                @event.LastSignup = @event.Date.Date + ts;

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,hostId,Title,Description,InviteTime,StartTime,LastSignup,Date")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TimeSpan ts = @event.InviteTime.TimeOfDay;
                    @event.InviteTime = @event.Date.Date + ts;

                    ts = @event.StartTime.TimeOfDay;
                    @event.StartTime = @event.Date.Date + ts;

                    ts = @event.LastSignup.TimeOfDay;
                    @event.LastSignup = @event.Date.Date + ts;

                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

   
        [HttpPost]
        public IActionResult Signup(Signup signup)
        {

            if(_context.Signups.FirstOrDefault(s => s.EventId==signup.EventId && s.UserId == signup.UserId)==null)
            {
                signup.User = _context.Users.FirstOrDefault(u => u.Id == signup.UserId);
                signup.Event = _context.Events.FirstOrDefault(e => e.Id == signup.EventId);
                signup.Sign = true;

                _context.Signups.Add(signup);
                _context.SaveChanges();
            }

            else
            {
                var signedUser = _context.Signups.FirstOrDefault(s => s.EventId == signup.EventId && s.UserId == signup.UserId);
                signedUser.Sign = true;
                signedUser.RoleDps = signup.RoleDps;
                signedUser.RoleHealer = signup.RoleHealer;
                signedUser.RoleTank = signup.RoleTank;
                _context.Signups.Update(signedUser);
                _context.SaveChanges();
            }

            return RedirectToAction("Details/"+ signup.EventId);
        }

        [HttpPost]
        public IActionResult UnSign(Signup signup)
        {

            if (_context.Signups.FirstOrDefault(s => s.EventId == signup.EventId && s.UserId == signup.UserId) == null)
            {
                signup.User = _context.Users.FirstOrDefault(u => u.Id == signup.UserId);
                signup.Event = _context.Events.FirstOrDefault(e => e.Id == signup.EventId);
                signup.Sign = false;

                _context.Signups.Add(signup);
                _context.SaveChanges();
            }

            else
            {
                var signedUser = _context.Signups.FirstOrDefault(s => s.UserId == signup.UserId && s.EventId == signup.EventId);
                signedUser.Sign = false;

                _context.Signups.Update(signedUser);
                _context.SaveChanges();
            }
            

            return RedirectToAction("Details/" + signup.EventId);
        }

        public async Task<IActionResult> Lineup(int? id)
        {
            LineupViewModel model = new LineupViewModel
            {
                Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id),
                Signups = _context.Signups.Where(s => s.EventId == id).ToList(),
                Lineups = _context.Lineups.Where(l => l.EventId == id).ToList(),
            };
            foreach (Signup s in model.Signups)
            {
                s.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == s.UserId);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Lineup(Lineup lineup)
        {

            if(_context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId) == null)
            {
                _context.Lineups.Add(lineup);
            }
            else
            {
                var existingLineup = _context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId);

                existingLineup.Role = lineup.Role;
                existingLineup.Group = lineup.Group;
                _context.Lineups.Update(existingLineup);
            }

            
            _context.SaveChanges();

            LineupViewModel model = new LineupViewModel
            {
                Event = _context.Events.FirstOrDefault(e => e.Id == lineup.EventId),
                Signups = _context.Signups.Where(s => s.EventId == lineup.EventId).ToList(),
                Lineups = _context.Lineups.Where(l => l.EventId == lineup.EventId).ToList(),
            };
            foreach (Signup s in model.Signups)
            {
                s.User = _context.Users.FirstOrDefault(u => u.Id == s.UserId);
            }

            return PartialView("_LineupPartial",model);
        }

        public IActionResult OfficerNote()
        {
            var model = new Lineup { };

            return PartialView("_OfficerNoteModal", model);
        }

        [HttpPost]
        public IActionResult OfficerNote(Lineup lineup)
        {


            var existingLineup = _context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId);
            existingLineup.OfficerNote = lineup.OfficerNote;    
            _context.Lineups.Update(existingLineup);
            _context.SaveChanges();

            LineupViewModel model = new LineupViewModel
            {
                Event = _context.Events.FirstOrDefault(e => e.Id == lineup.EventId),
                Signups = _context.Signups.Where(s => s.EventId == lineup.EventId).ToList(),
                Lineups = _context.Lineups.Where(l => l.EventId == lineup.EventId).ToList(),
            };
            foreach (Signup s in model.Signups)
            {
                s.User = _context.Users.FirstOrDefault(u => u.Id == s.UserId);
            }

            return PartialView("_LineupPartial", model);
        }
    }
}
