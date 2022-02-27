using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IV.AccesoDatos.Data;
using IV.Modelos;

namespace InventarioVenta.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BodegasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Bodegas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bodegas.ToListAsync());
        }

        // GET: Admin/Bodegas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodegas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodega == null)
            {
                return NotFound();
            }

            return View(bodega);
        }

        // GET: Admin/Bodegas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Bodegas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Estado")] Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bodega);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bodega);
        }

        // GET: Admin/Bodegas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodegas.FindAsync(id);
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }

        // POST: Admin/Bodegas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado")] Bodega bodega)
        {
            if (id != bodega.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodega);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodegaExists(bodega.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bodega);
        }

        // GET: Admin/Bodegas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodegas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodega == null)
            {
                return NotFound();
            }

            return View(bodega);
        }

        // POST: Admin/Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodega = await _context.Bodegas.FindAsync(id);
            _context.Bodegas.Remove(bodega);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodegaExists(int id)
        {
            return _context.Bodegas.Any(e => e.Id == id);
        }
    }
}
