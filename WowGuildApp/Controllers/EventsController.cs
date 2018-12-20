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


        //Get: Events/Index
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

            var @signups = _context.Signups.Where(s => s.EventId == @event.Id).Include(s=>s.User).Include(s => s.Character).Include(s => s.Specializations).ToList();

            var @lineups = _context.Lineups.Where(l => l.EventId == @event.Id).ToList();

            var currentUser = await GetCurrentUserAsync();

            var @characters = _context.Characters.Where(c => c.UserId == currentUser.Id).ToList();
            
            EventDetailViewModel model = new EventDetailViewModel
            {
                Event = @event,
                Host = @host,
                Signups = @signups,
                Lineups = lineups,
                ConfirmedLineup = @event.ConfirmedLineup,
                Characters = characters,
                
            };


            return View(model);
        }

        // GET: Events/Create/2018-12-13
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
                

                //fixes the date values of the time properties
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
                    //fixes the date values of the time properties

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

        //POST: Events/Signup/
        [HttpPost]
        public IActionResult Signup(Signup signup)
        {
            //stops signups after last signup time
            if(_context.Events.FirstOrDefault(e=>e.Id==signup.EventId).LastSignup> DateTime.Now)
            {
                var signedUser = _context.Signups.Where(s => s.EventId == signup.EventId && s.UserId == signup.UserId).ToList();
                var signedCharacter = _context.Signups.FirstOrDefault(s => s.EventId == signup.EventId && s.UserId == signup.UserId && s.CharacterId == signup.CharacterId);
                //if signup doesnt exist, create new. else update
                if (signedCharacter==null)
                {
                    signup.User = _context.Users.FirstOrDefault(u => u.Id == signup.UserId);
                    signup.Event = _context.Events.FirstOrDefault(e => e.Id == signup.EventId);
                    signup.Character = _context.Characters.FirstOrDefault(c => c.Id == signup.CharacterId);
                    signup.Sign = true;

                    _context.Signups.Add(signup);
                    _context.SaveChanges();
                }

                else
                {
                    foreach(Signup s in signedUser)
                    {
                        //if user has unsigned characters, remove them from unsigned list
                        if(s.CharacterId == signup.CharacterId)
                        {
                            signedCharacter.Sign = true;
                            signedCharacter.RoleDps = signup.RoleDps;
                            signedCharacter.RoleHealer = signup.RoleHealer;
                            signedCharacter.RoleTank = signup.RoleTank;
                            _context.Signups.Update(signedCharacter);
                        }
                        else if(!s.Sign)
                        {
                            _context.Signups.Remove(s);
                        }
                    }

                    
                    _context.SaveChanges();
                }

            }


            return RedirectToAction("Details/"+ signup.EventId);
        }

        [HttpPost]
        public IActionResult UnSign(Signup signup)
        {

            var signups = _context.Signups.Where(s => s.EventId == signup.EventId && s.UserId == signup.UserId).ToList();
            //if signup doesnt exist, create as unsigned. else update
            if (signups.Count() <= 0)
            {
                signup.User = _context.Users.FirstOrDefault(u => u.Id == signup.UserId);
                signup.Event = _context.Events.FirstOrDefault(e => e.Id == signup.EventId);
                signup.Character = _context.Characters.FirstOrDefault(c => c.UserId == signup.UserId && c.Main == true);
                signup.Sign = false;

                _context.Signups.Add(signup);
                _context.SaveChanges();
            }

            else
            {

                foreach(Signup s in signups)
                {
                    s.Sign = false;
                    _context.Signups.Update(s);
                    var tempLineup = _context.Lineups.FirstOrDefault(l => l.UserId == signup.UserId && l.EventId == signup.EventId);
                    if (tempLineup != null)
                    {
                        _context.Lineups.Remove(tempLineup);
                    }
                }
                
                _context.SaveChanges();
            }
            

            return RedirectToAction("Details/" + signup.EventId);
        }

        //GET: Events/Lineup/3
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

        //POST: Events/Lineup/3
        [HttpPost]
        public IActionResult AddToLineup(Lineup lineup)
        {
            //if lineup doesnt exist, create new lineup. else update
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

            //Reloads model for partial view
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


        //DELETE: Events/Lineup/3
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

        //POST: Events/Lineup/3
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

            //Reloads model for partial view
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


        //POST Events/Lineup/3
        [HttpPost]
        public IActionResult SignupNote(Signup signup)
        {
            //stops signup note if after invite time
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

            //Reloads model for partial view

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

        
        //GET: Events/ConfimLineup/3
        public IActionResult ConfirmLineup(int id)
        {
            Event model = _context.Events.FirstOrDefault(e => e.Id == id);

            return View(model);
        }

        //POST: Events/ConfirmLineup/3
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
