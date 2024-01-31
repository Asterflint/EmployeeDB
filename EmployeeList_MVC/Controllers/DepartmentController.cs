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
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        // GET: Department/AddOrEdit(Insert)
        // GET: Department/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Department());
            else
            {
                var DepartmentModel = await _context.Departments.FindAsync(id);
                if (DepartmentModel == null)
                {
                    return NotFound();
                }
                return View(DepartmentModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("ID,DepartmentName,Abbreviation")] Department DepartmentModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0 )
                {
                    
                    _context.Add(DepartmentModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(DepartmentModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        if (!DepartmentModelExists(DepartmentModel.ID))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Departments.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", DepartmentModel) });
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DepartmentModel = await _context.Departments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (DepartmentModel == null)
            {
                return NotFound();
            }

            return View(DepartmentModel);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DepartmentModel = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(DepartmentModel);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Departments.ToList()) });
        }

        private bool DepartmentModelExists(int id)
        {
            return _context.Departments.Any(e => e.ID == id);
        }
    }
}
