﻿using System;
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

            var @lineups = _context.Lineups.Where(l => l.EventId == @event.Id).ToList();

            EventDetailViewModel model = new EventDetailViewModel
            {
                Event = @event,
                Host = @host,
                Signups = @signups,
                Lineups = lineups,
                ConfirmedLineup = @event.ConfirmedLineup,
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
            if(_context.Events.FirstOrDefault(e=>e.Id==signup.EventId).LastSignup> DateTime.Now)
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

                var tempLineup = _context.Lineups.FirstOrDefault(l => l.UserId == signup.UserId && l.EventId == signup.EventId);
                if(tempLineup != null)
                {
                    _context.Lineups.Remove(tempLineup);
                }

                _context.Signups.Update(signedUser);
                _context.SaveChanges();
            }
            

            return RedirectToAction("Details/" + signup.EventId);
        }

        public async Task<IActionResult> Lineup(int? id)
        {

            if(_context.Events.FirstOrDefault(e => e.Id == id).LastSignup > DateTime.Now)
            {
                return RedirectToAction("Details/" + id);
            }

            else
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

        }

        [HttpPost]
        public IActionResult AddToLineup(Lineup lineup)
        {

            if(_context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId) == null)
            {
                lineup.Note = _context.Signups.FirstOrDefault(s => s.EventId == lineup.EventId && s.UserId == lineup.UserId).Note;
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

        [HttpDelete]
        public IActionResult RemoveFromLineup(Lineup lineup)
        {
            var temp = _context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId);
            if (temp != null)
            {
                _context.Lineups.Remove(temp);
                _context.SaveChanges();
            }

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

        [HttpPost]
        public IActionResult OfficerNote(Lineup lineup)
        {
            if(_context.Events.FirstOrDefault(e => e.Id==lineup.EventId).InviteTime>DateTime.Now)
            {
                var dbLineup = _context.Lineups.FirstOrDefault(l => l.EventId == lineup.EventId && l.UserId == lineup.UserId);
                dbLineup.OfficerNote = lineup.OfficerNote;    
                _context.Lineups.Update(dbLineup);
                _context.SaveChanges();
            }

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

        [HttpPost]
        public IActionResult SignupNote(Signup signup)
        {
            if(_context.Events.FirstOrDefault(e => e.Id == signup.EventId).InviteTime>DateTime.Now)
            {
                var dbSignup = _context.Signups.FirstOrDefault(s => s.EventId == signup.EventId && s.UserId == signup.UserId);
                var dbLineup = _context.Lineups.FirstOrDefault(l => l.EventId == signup.EventId && l.UserId == signup.UserId);
                dbSignup.Note = signup.Note;
                _context.Signups.Update(dbSignup);

                if(dbLineup != null)
                {
                    dbLineup.Note = signup.Note;
                    _context.Lineups.Update(dbLineup);
                }

                _context.SaveChanges();

            }


            var @event = _context.Events.FirstOrDefault(e => e.Id == signup.EventId);
            var @signups = _context.Signups.Where(s => s.EventId == @event.Id).ToList();
            var @lineups = _context.Lineups.Where(l => l.EventId == @event.Id).ToList();

            var model = new EventDetailViewModel
            {
                Event = @event,
                Host = @event.host,
                ConfirmedLineup = @event.ConfirmedLineup,
                Lineups = @lineups,
                Signups = @signups,
            };

            foreach(Signup s in @signups)
            {
                s.User = _context.Users.FirstOrDefault(u => u.Id == s.UserId);
            }

           foreach(Lineup l in @lineups)
            {
                l.User = _context.Users.FirstOrDefault(u => u.Id == l.UserId);
            }


            return PartialView("_DetailsPartial", model);
        }

        
        public IActionResult ConfirmLineup(int id)
        {
            Event model = _context.Events.FirstOrDefault(e => e.Id == id);

            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmLineup(Event Event)
        {

            var temp = _context.Events.FirstOrDefault(e => e.Id == Event.Id);

            temp.ConfirmedLineup = Event.ConfirmedLineup;

            temp.Lineup = _context.Lineups.Where(l => l.EventId == Event.Id).ToList();

            _context.Update(temp);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = Event.Id });
        }
    }
}
