using MetallurgyApp.Web.Data;
using MetallurgyApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MetallurgyApp.Web.Controllers;

public class ProductionAreasController : Controller
{
    private readonly AppDbContext _db;
    public ProductionAreasController(AppDbContext db) => _db = db;

    // GET: /ProductionAreas?search=...&workshop=...
    public async Task<IActionResult> Index(string? search, string? workshop)
    {
        var query = _db.ProductionAreas
            .Include(a => a.Equipments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Name.Contains(search) || a.Head.Contains(search));

        if (!string.IsNullOrWhiteSpace(workshop))
            query = query.Where(a => a.Workshop == workshop);

        ViewBag.Search = search;
        ViewBag.Workshop = workshop;
        ViewBag.Workshops = await _db.ProductionAreas
            .Select(a => a.Workshop).Distinct().ToListAsync();

        return View(await query.AsNoTracking().ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var area = await _db.ProductionAreas
            .Include(a => a.Equipments)
            .FirstOrDefaultAsync(a => a.Id == id);
        return area is null ? NotFound() : View(area);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductionArea area)
    {
        if (!ModelState.IsValid) return View(area);
        _db.Add(area);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var area = await _db.ProductionAreas.FindAsync(id);
        return area is null ? NotFound() : View(area);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductionArea area)
    {
        if (id != area.Id) return BadRequest();
        if (!ModelState.IsValid) return View(area);

        _db.Update(area);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var area = await _db.ProductionAreas
            .Include(a => a.Equipments)
            .FirstOrDefaultAsync(a => a.Id == id);
        return area is null ? NotFound() : View(area);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var area = await _db.ProductionAreas.FindAsync(id);
        if (area != null)
        {
            _db.ProductionAreas.Remove(area);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}