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
using EmployeeList_MVC.Data;
using EmployeeList_MVC.Data;
using EmployeeList_MVC;
using static EmployeeList_MVC.Helper;

namespace EmployeeList_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/AddOrEdit(Insert)
        // GET: Employee/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
            {
                var EmployeeModel = await _context.Employees.FindAsync(id);
                if (EmployeeModel == null)
                {
                    return NotFound();
                }
                return View(EmployeeModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("FirstName,LastName,Email")] Employee EmployeeModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    //EmployeeModel.Date = DateTime.Now;
                    _context.Add(EmployeeModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(EmployeeModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        if (!EmployeeModelExists(EmployeeModel.ID))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", EmployeeModel) });
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var EmployeeModel = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (EmployeeModel == null)
            {
                return NotFound();
            }

            return View(EmployeeModel);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var EmployeeModel = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(EmployeeModel);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employees.ToList()) });
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
