using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CheckPoint2App.Data;
using CheckPoint2App.Models;

namespace CheckPoint2App.Controllers;

[Authorize]
public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Student
    // Supports searching (searchString) and sorting (sortOrder)
    public IActionResult Index(string searchString, string sortOrder)
    {
        // Set up sorting parameters for Razor view headers
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["CourseSortParm"] = sortOrder == "Course" ? "course_desc" : "Course";
        ViewData["CurrentFilter"] = searchString;

        var students = _context.Students.AsQueryable();

        // 1. Search Filter Logic
        if (!string.IsNullOrEmpty(searchString))
        {
            students = students.Where(s => s.FirstName.Contains(searchString) ||
                                           s.LastName.Contains(searchString) ||
                                           s.Email.Contains(searchString) ||
                                           s.Course.Contains(searchString));
        }

        // 2. Sorting Logic
        students = sortOrder switch
        {
            "name_desc" => students.OrderByDescending(s => s.LastName),
            "Course" => students.OrderBy(s => s.Course),
            "course_desc" => students.OrderByDescending(s => s.Course),
            _ => students.OrderBy(s => s.LastName)
        };

        return View(students.ToList());
    }

    // GET: /Student/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Student/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["Success"] = "Student record created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: /Student/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // POST: /Student/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Students.Update(student);
            _context.SaveChanges();

            TempData["Success"] = "Student details updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // POST: /Student/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();

            TempData["Success"] = "Student deleted successfully!";
        }
        else
        {
            TempData["Error"] = "Student record not found.";
        }
        return RedirectToAction(nameof(Index));
    }
}