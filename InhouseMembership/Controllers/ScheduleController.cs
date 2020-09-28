using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InhouseMembership.Data;
using InhouseMembership.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace InhouseMembership.Controllers
{
    [Authorize(Roles = "Admin, Coach, Member")]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        
        public ScheduleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Schedule
        [Authorize(Roles = "Admin, Coach, Member")]
        public async Task<IActionResult> Index()
        {

            var scheduleList = await _context.Schedules.ToListAsync();
                 
            // ensure the coach logged in can only see the upcoming schedule that is hosted by himself
            if (User.IsInRole("Coach"))
            {
                return View(scheduleList.Where(s => s.CoachId.Equals(_userManager.GetUserId(HttpContext.User))));
            }
            // members and admins can see all schedules
            else {
                return View(scheduleList);
            }        
        }


        // GET: Schedule/Details/5
        [Authorize(Roles = "Admin, Coach, Member")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }


        // redirect to the coach profile page, it is used to access to a coach profile from the schedule detail page
        [Authorize(Roles = "Admin, Coach, Member")]
        public IActionResult CoachProfile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // get the coach profile id based on the coach id passed in the function
            CoachProfile coachProfile = _context.CoachProfiles.Where(p => p.CoachId == id).FirstOrDefault();

            if (coachProfile == null)
            {
                return RedirectToAction("NoProfile", "CoachProfile");
            }
            var profileId = coachProfile.CoachProfileId;
        
            // redirect to the profile page using the profile id
            return Redirect("../../CoachProfile/Details/" + profileId);
        }


        // Enroll in a schedule
        [Authorize(Roles = "Admin, Member")]
        public IActionResult CreateEnrollment(string id)
        {
            // generate random enrollment Id
            Random rnd = new Random();
            int randomNumber = rnd.Next();
            string strRandomNumber = randomNumber.ToString();
            // the data will be passed to the create action in 'Enrollment' controller using TempData to create a new enrollment
            var data = new Dictionary<string, string>() {         
            {"EnrollmentId", strRandomNumber},
            {"ScheduleId", id},
            {"MemberId", _userManager.GetUserAsync(User).Result.Id},
            //{"Enrollments", _userManager.GetUserAsync(User).Result},
            };
            TempData["mydata"] = data;
            //TempData["mydata"] = JsonConvert.SerializeObject(data);
            return RedirectToAction("Create", "Enrollment");

        }


        // GET: Schedule/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // pass a list of coaches to ViewData, which can be used to Create a dropdown select menu for matching coach and schedule 
            ViewData["coaches"] = _userManager.GetUsersInRoleAsync("Coach").Result.ToList();
            return View();
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ScheduleId,CoachId,EventName,EventDate,Location")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                // generate random Schedule Id
                Random rnd = new Random();
                int randomNumber = rnd.Next();
                string strRandomNumber = randomNumber.ToString();
                schedule.ScheduleId = strRandomNumber;
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }


        // GET: Schedule/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }




        // POST: Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("ScheduleId,CoachId,EventName,EventDate,Location")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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
            return View(schedule);
        }

 
        
        // GET: Schedule/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(string id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }
    }
}
