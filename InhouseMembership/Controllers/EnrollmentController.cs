using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InhouseMembership.Data;
using InhouseMembership.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace InhouseMembership.Controllers
{
    [Authorize(Roles = "Admin, Coach, Member")]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
         
        public EnrollmentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;   
            _context = context;
        }


        // GET: Enrollment
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Index()
        {
            // use tuple to return lists of Schedules and Enrollments to the View 
            // this is to show the schedule info and who enrolled it
            var tupleModel = new Tuple<IEnumerable<Schedule>, IEnumerable<Enrollment>>(await _context.Schedules.ToListAsync(), await _context.Enrollments.ToListAsync());
            return View(tupleModel);
        
        }

        // GET: Enrollment/Details/5
        // get details of a Enrollment
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollment/Create
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> Create([Bind("EnrollmentId,ScheduleId,MemberId")] Enrollment enrollment)
        {
            // use data recieved from Enroll action in Schedule controller to create an Enrollment
            var data = TempData["mydata"] as Dictionary<string, string>;     
            enrollment.EnrollmentId = data["EnrollmentId"];
            enrollment.ScheduleId = data["ScheduleId"];
            enrollment.MemberId = data["MemberId"];
  
            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Schedule");
        }



        // GET: Enrollment/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }


        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("EnrollmentId,ScheduleId,MemberId")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId))
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
            return View(enrollment);
        }



        // GET: Enrollment/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }


        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(string id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}
