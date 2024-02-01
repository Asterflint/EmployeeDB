﻿using System;
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

namespace EmployeeList_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetJobTitlesForDepartment(int departmentID)
        {
            var jobTitles = _context.JobTitles.Where(j => j.DepartmentID == departmentID).ToList();
            var jobTitleList = jobTitles.Select(j => new { value = j.ID, text = j.JobTitleName });

            return Json(jobTitleList);
        }

        // GET: Employee/Show/5
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var employee = await _context.Employees
            .Include(e => e.JobTitle)
            .ThenInclude(jt => jt.Department)
            .FirstOrDefaultAsync(m => m.ID == id);

            //var employee = employees.FirstOrDefaultAsync(m => m.ID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public async Task<IActionResult> Index(int pg = 1, int entriesPerPage = 5, string searchQuery = "")
        {
            var jobTitles = _context.JobTitles.ToList();
            ViewData["JobTitles"] = new SelectList(jobTitles, "ID", "JobTitleName");

            IQueryable<Employee> employeesQuery = _context.Employees.Include(e => e.JobTitle).ThenInclude(jt => jt.Department); // This includes the Department related to the JobTitle

            // Apply search filter if searchQuery is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                employeesQuery = employeesQuery.Where(e =>
                    EF.Functions.Like(e.FirstName, $"%{searchQuery}%") ||
                    EF.Functions.Like(e.LastName, $"%{searchQuery}%") ||
                    EF.Functions.Like(e.Email, $"%{searchQuery}%"));

            }

            var employees = await employeesQuery.ToListAsync();

            //const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int pageSize = entriesPerPage;
            int recsCount = employees.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = employees.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = entriesPerPage;
            ViewData["SearchQuery"] = searchQuery;

            return View(data);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            var jobTitles = _context.JobTitles.ToList();
            ViewData["JobTitles"] = new SelectList(jobTitles, "ID", "JobTitleName");

            var department = _context.Departments.ToList();
            ViewData["Departments"] = new SelectList(department, "ID", "DepartmentName");

            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

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
        public async Task<IActionResult> AddOrEdit(int id, [Bind("ID,NIK,FirstName,LastName,Email,JobTitleID,HireDate,Gender,DateOfBirth,PlaceOfBirth,Address,Phone")] Employee EmployeeModel)
        {
            if (ModelState.IsValid)
            {
                // Store the entriesPerPage in ViewData for access in the view
                ViewData["EntriesPerPage"] = 5;

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

                //var employees = _context.Employees.ToList();

                IQueryable<Employee> employeesQuery = _context.Employees.Include(e => e.JobTitle).ThenInclude(jt => jt.Department); // This includes the Department related to the JobTitle

                var employees = await employeesQuery.ToListAsync();


                const int pageSize = 5;
                int pg = 1;
                if (pg < 1)
                {
                    pg = 1;
                }

                int recsCount = employees.Count;
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = employees.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", data) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", EmployeeModel) });
        }

        //GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

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
            // Store the entriesPerPage in ViewData for access in the view
            ViewData["EntriesPerPage"] = 5;

            var EmployeeModel = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(EmployeeModel);
            await _context.SaveChangesAsync();

            var employees = await _context.Employees.ToListAsync();
            const int pageSize = 5;
            int pg = 1;
            if (pg < 1)
            {
                pg = 1;
            }

            int recsCount = employees.Count;
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = employees.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View("_ViewAll", data);

            //return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll",data) });
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
