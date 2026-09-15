using MetallurgyApp.Web.Data;
using MetallurgyApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MetallurgyApp.Web.Controllers;

public class EquipmentsController : Controller
{
    private readonly AppDbContext _db;
    public EquipmentsController(AppDbContext db) => _db = db;

    // GET: /Equipments?search=&state=&areaId=
    public async Task<IActionResult> Index(string? search, EquipmentState? state, int? areaId)
    {
        var query = _db.Equipments.Include(e => e.ProductionArea).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search) || e.Model.Contains(search));

        if (state.HasValue)
            query = query.Where(e => e.State == state);

        if (areaId.HasValue)
            query = query.Where(e => e.ProductionAreaId == areaId);

        await PopulateViewBagsAsync(search, state, areaId);
        return View(await query.AsNoTracking().ToListAsync());
    }

    private async Task PopulateViewBagsAsync(string? search, EquipmentState? state, int? areaId)
    {
        ViewBag.Search = search;
        ViewBag.State = state;
        ViewBag.AreaId = areaId;
        ViewBag.Areas = new SelectList(
            await _db.ProductionAreas.AsNoTracking().ToListAsync(),
            "Id", "Name", areaId);
    }

    public async Task<IActionResult> Details(int id)
    {
        var e = await _db.Equipments.Include(x => x.ProductionArea)
            .FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : View(e);
    }

    public async Task<IActionResult> Create()
    {
        await LoadAreasAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Equipment equipment)
    {
        if (!ModelState.IsValid)
        {
            await LoadAreasAsync(equipment.ProductionAreaId);
            return View(equipment);
        }
        _db.Add(equipment);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var e = await _db.Equipments.FindAsync(id);
        if (e is null) return NotFound();
        await LoadAreasAsync(e.ProductionAreaId);
        return View(e);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Equipment equipment)
    {
        if (id != equipment.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            await LoadAreasAsync(equipment.ProductionAreaId);
            return View(equipment);
        }
        _db.Update(equipment);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Equipments.Include(x => x.ProductionArea)
            .FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : View(e);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var e = await _db.Equipments.FindAsync(id);
        if (e != null)
        {
            _db.Equipments.Remove(e);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadAreasAsync(int? selected = null) =>
        ViewBag.Areas = new SelectList(
            await _db.ProductionAreas.AsNoTracking().ToListAsync(),
            "Id", "Name", selected);
}