using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InhouseMembership.Data;
using InhouseMembership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InhouseMembership.Controllers
{
    [Authorize(Roles = "Admin, Member")]
    public class UserController : Controller
    {

        UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAdmins()
        {
            // get a list of admins
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            // pass the admins to Index view page
            return View("Index", admins);
        }


        [Authorize(Roles = "Admin, Member")]
        public async Task<ActionResult> GetCoaches()
        {
            // get a list of coaches
            var coaches = await _userManager.GetUsersInRoleAsync("Coach");

            // pass the coaches to Index view page
            return View("Index", coaches);
            
        }



        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetMembers()
        {
            // get a list of members
            var members = await _userManager.GetUsersInRoleAsync("Member");

            // pass the members to Index view page
            return View("Index", members);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return Redirect("../Identity/Account/Register");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
                //.FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [Authorize(Roles = "Admin, Member")]
        // redirect to the coach profile page
        public IActionResult CoachProfile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // get the coach profile id based on the coach id passed into the function
            CoachProfile coachProfile = _context.CoachProfiles.Where(p => p.CoachId == id).FirstOrDefault();

            if (coachProfile == null)
            {
                
                return RedirectToAction("NoProfile", "CoachProfile");

            }
            var profileId = coachProfile.CoachProfileId;


            // redirect to the profile page using the profile id
            return Redirect("../../CoachProfile/Details/" + profileId);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var User = await _userManager.FindByIdAsync(id);
            Console.WriteLine("User: " + User);
            if (User == null)
            {
                return NotFound();
            }
            return View(User);
        }

        // POST: Enrollment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Email, PhoneNumber")] ApplicationUser applicationUser)
        {
            
            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser();
                user = _userManager.FindByIdAsync(id).Result;
                await _userManager.SetUserNameAsync(user, applicationUser.UserName);
                await _userManager.SetEmailAsync(user, applicationUser.Email);
                await _userManager.SetPhoneNumberAsync(user, applicationUser.PhoneNumber);
                var userRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
                // check the role of the user that is being deleted, then retrun differnet list accordingly

                if (userRole == "Admin")
                {
                    return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Admin"));
                }
                else if (userRole == "Coach")
                {

                    return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Coach"));

                }
                else if (userRole == "Member")
                {

                    return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Member"));
                }




                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            ApplicationUser user = new ApplicationUser();
            user = _userManager.FindByIdAsync(id).Result;
            var userRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            await _userManager.DeleteAsync(user);
            // check the role of the user that is being deleted, then retrun differnet list accordingly

            if (userRole == "Admin")
            {
                return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Admin"));
            }
            else if (userRole == "Coach")
            {
                return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Coach"));

            }
            else if (userRole == "Member")
            {
                return View(nameof(Index), await _userManager.GetUsersInRoleAsync("Member"));
            }



            return RedirectToAction(nameof(Schedule));

        }
    }
}
