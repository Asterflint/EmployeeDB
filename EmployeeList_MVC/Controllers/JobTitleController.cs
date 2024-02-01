using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeList_MVC.Models;
using static EmployeeList_MVC.Helper;
using EmployeeList_MVC;
using System.Data.Entity.Infrastructure;
using EmployeeList_MVC.Data;

namespace EmployeeList_MVC.Controllers
{
    public class JobTitleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobTitleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobTitle
        public async Task<IActionResult> Index(int pg = 1, int entriesPerPage = 5, string searchQuery = "")
        {
            var departments = _context.Departments.ToList();
            ViewData["Departments"] = new SelectList(departments, "ID", "DepartmentName");

            IQueryable<JobTitle> jobtitleQuery = _context.JobTitles.Include(e => e.Department); // This includes the Department related to the JobTitle

            // Apply search filter if searchQuery is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                jobtitleQuery = jobtitleQuery.Where(e =>
                    EF.Functions.Like(e.JobTitleName, $"%{searchQuery}%") ||
                    EF.Functions.Like(e.Department.DepartmentName, $"%{searchQuery}%"));

            }

            var jobtitles = await jobtitleQuery.ToListAsync();

            //const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int pageSize = entriesPerPage;
            int recsCount = jobtitles.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = jobtitles.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = entriesPerPage;
            ViewData["SearchQuery"] = searchQuery;

            return View(data);
        }

        // GET: JobTitle/AddOrEdit(Insert)
        // GET: JobTitle/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            var department = _context.Departments.ToList();
            ViewData["Departments"] = new SelectList(department, "ID", "DepartmentName");

            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

            if (id == 0)
                return View(new JobTitle());
            else
            {
                var JobTitleModel = await _context.JobTitles
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ID == id);

                if (JobTitleModel == null)
                {
                    return NotFound();
                }
                return View(JobTitleModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("ID,JobTitleName,DepartmentID")] JobTitle JobTitleModel)
        {
            var department = _context.Departments.ToList();
            ViewData["Departments"] = new SelectList(department, "ID", "DepartmentName");

            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;


            if (ModelState.IsValid)
            {

                //Insert
                if (id == 0)
                {
                    _context.Add(JobTitleModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(JobTitleModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        if (!JobTitleModelExists(JobTitleModel.ID))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }

                IQueryable<JobTitle> jobtitlesQuery = _context.JobTitles.Include(e => e.Department); // This includes the Department related to the JobTitle

                var jobtitles = await jobtitlesQuery.ToListAsync();


                const int pageSize = 5;
                int pg = 1;
                if (pg < 1)
                {
                    pg = 1;
                }

                int recsCount = jobtitles.Count;
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = jobtitles.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", data) });
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", JobTitleModel) });
        }

        // GET: JobTitle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

            if (id == null)
            {
                return NotFound();
            }

            var JobTitleModel = await _context.JobTitles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (JobTitleModel == null)
            {
                return NotFound();
            }

            return View(JobTitleModel);
        }

        // POST: JobTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

            var JobTitleModel = await _context.JobTitles.FindAsync(id);
            _context.JobTitles.Remove(JobTitleModel);
            await _context.SaveChangesAsync();

            var jobtitles = await _context.JobTitles.ToListAsync();
            const int pageSize = 5;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = jobtitles.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = jobtitles.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View("_ViewAll", data);
        }

        private bool JobTitleModelExists(int id)
        {
            return _context.JobTitles.Any(e => e.ID == id);
        }
    }
}
