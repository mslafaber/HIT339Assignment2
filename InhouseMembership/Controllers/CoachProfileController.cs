using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InhouseMembership.Data;
using InhouseMembership.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace InhouseMembership.Controllers
{
    [Authorize(Roles = "Admin, Coach, Member")]
    public class CoachProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public CoachProfileController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        // GET: CoachProfile
        [Authorize(Roles = "Admin, Coach, Member")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CoachProfiles.Include(c => c.Coach);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: CoachProfile/Details/5
        // this action shows coach profile
        [Authorize(Roles = "Admin, Coach, Member")]
        public async Task<IActionResult> Details(int? id)
        {

            CoachProfile coachProfile = new CoachProfile();

            if (User.IsInRole("Coach"))
            {
                // show the profile of the coach that currently logged in  
                var CoachId = _userManager.GetUserId(User);
                coachProfile = await _context.CoachProfiles
                    .Include(c => c.Coach)
                    .FirstOrDefaultAsync(m => m.CoachId == CoachId);
                // redirect to no profile page if the coach has not created a profile yet
                if (coachProfile == null)
                {
                    return RedirectToAction("NoProfile");
                }

            }
            // if the current usr is not a coach, find the profile using id that is passed in this function
            else 
            {
                coachProfile = await _context.CoachProfiles
                           .Include(c => c.Coach)
                           .FirstOrDefaultAsync(m => m.CoachProfileId == id);
            }

          return View(coachProfile);
        }




        // returns no profile page if the coach has not created one 
        [Authorize(Roles = "Admin, Coach, Member")]
        public IActionResult NoProfile() 
        {
            return View();
        }





        // GET: CoachProfile/Create
        [Authorize(Roles = "Admin, Coach")]
        public IActionResult Create()
        {   
            return View();
        }

        // POST: CoachProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Create([Bind("CoachProfileId,CoachId,Education,Interests,Experience,Skills,Biography,ImageFile")] CoachProfile coachProfile)
        {
            if (ModelState.IsValid)
            {
                // upload image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(coachProfile.ImageFile.FileName);
                string extension = Path.GetExtension(coachProfile.ImageFile.FileName);
                coachProfile.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await coachProfile.ImageFile.CopyToAsync(fileStream);
                
                }

                // assign the coach id of the coach currently logged in to the new coach profile
                coachProfile.CoachId = _userManager.GetUserId(User);
                    _context.Add(coachProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            
            return View(coachProfile);
        }



        // GET: CoachProfile/Edit/5
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var coachProfile = await _context.CoachProfiles.FindAsync(id);
            if (coachProfile == null)
            {
                return NotFound();
            }
           
            return View(coachProfile);
        }


        // POST: CoachProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Edit(int id, [Bind("CoachProfileId,CoachId,Education,Interests,Experience,Skills,Biography,ImageFile")] CoachProfile coachProfile)
        {

            coachProfile.CoachId = _userManager.GetUserId(User);
            if (id != coachProfile.CoachProfileId)
            {
                return NotFound();
            }
            // upload image to wwwroot/image
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(coachProfile.ImageFile.FileName);
            string extension = Path.GetExtension(coachProfile.ImageFile.FileName);
            coachProfile.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/image/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await coachProfile.ImageFile.CopyToAsync(fileStream);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coachProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachProfileExists(coachProfile.CoachProfileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }           
            return View(coachProfile);
        }



        // GET: CoachProfile/Delete/5
        [Authorize(Roles = "Admin, Coach")]
        public async Task<IActionResult> Delete(int id)
        {

           // find the coach profile of current coach logged in, then delete it
            var coachProfile = await _context.CoachProfiles.FindAsync(id);
            if (coachProfile == null)
            {
                return NotFound();
            }           
            _context.CoachProfiles.Remove(coachProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
            
        }

        
        private bool CoachProfileExists(int id)
        {
            return _context.CoachProfiles.Any(e => e.CoachProfileId == id);
        }
    }
}
