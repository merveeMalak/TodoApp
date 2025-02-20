using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TodoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TodoController : Controller
{
    private readonly ApplicationDbContext _context = new ApplicationDbContext();

    public ActionResult Index(TodoStatus? status = null, bool showArchived = false)
    {
        var todos = _context.Todos.AsQueryable();

        if (showArchived)
        {
            todos = todos.Where(t => t.IsArchived);
        }
        else if (status.HasValue)
        {
            todos = todos.Where(t => t.Status == status.Value && !t.IsArchived);
        }

        ViewBag.Status = status;
        ViewBag.ShowArchived = showArchived; // Eğer null ise false ata
        return View(todos.ToList());
    }
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Todo todo)
    {
        if (ModelState.IsValid)
        {
            if (todo.DueDate.HasValue && todo.DueDate.Value < DateTime.Now)
            {
                ModelState.AddModelError("DueDate", "The due date must be in the future");
                return View(todo);
            }
            _context.Todos.Add(todo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(todo);
    }
    public ActionResult Edit(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo == null)
        {
            return HttpNotFound();
        }
        return View(todo);
    }

    // Edit Formunu Kaydet
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Todo todo)
    {
        if (ModelState.IsValid)
        {
            var existingTodo = _context.Todos.Find(todo.Id);
            if (existingTodo != null)
            {
                existingTodo.Name = todo.Name;
                existingTodo.Description = todo.Description;
                existingTodo.DueDate = todo.DueDate;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        return View(todo);
    }
    public ActionResult ChangeStatus(int id, TodoStatus newStatus)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            todo.Status = newStatus;
            _context.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public ActionResult Archive(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            todo.IsArchived = true;
            _context.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }


}