using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projetoRedeSocial.Models;

namespace projetoRedeSocial.Controllers
{
    public class SeguidoresController : Controller
    {
        private readonly Contexto _context;

        public SeguidoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Seguidores
        public async Task<IActionResult> Index()
        {
            var contexto = _context.seguidores.Include(s => s.seguidoUsuario).Include(s => s.usuarioSeguidor);
            return View(await contexto.ToListAsync());
        }

        // GET: Seguidores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.seguidores == null)
            {
                return NotFound();
            }

            var seguidores = await _context.seguidores
                .Include(s => s.seguidoUsuario)
                .Include(s => s.usuarioSeguidor)
                .FirstOrDefaultAsync(m => m.idSeguidor == id);
            if (seguidores == null)
            {
                return NotFound();
            }

            return View(seguidores);
        }

        // GET: Seguidores/Create
        public IActionResult Create()
        {
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            ViewData["idUsuarioSeguidor"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            return View();
        }

        // POST: Seguidores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idSeguidor,idUsuario,idUsuarioSeguidor")] Seguidores seguidores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seguidores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuario);
            ViewData["idUsuarioSeguidor"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuarioSeguidor);
            return View(seguidores);
        }

        // GET: Seguidores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.seguidores == null)
            {
                return NotFound();
            }

            var seguidores = await _context.seguidores.FindAsync(id);
            if (seguidores == null)
            {
                return NotFound();
            }
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuario);
            ViewData["idUsuarioSeguidor"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuarioSeguidor);
            return View(seguidores);
        }

        // POST: Seguidores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idSeguidor,idUsuario,idUsuarioSeguidor")] Seguidores seguidores)
        {
            if (id != seguidores.idSeguidor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seguidores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguidoresExists(seguidores.idSeguidor))
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
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuario);
            ViewData["idUsuarioSeguidor"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", seguidores.idUsuarioSeguidor);
            return View(seguidores);
        }

        // GET: Seguidores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.seguidores == null)
            {
                return NotFound();
            }

            var seguidores = await _context.seguidores
                .Include(s => s.seguidoUsuario)
                .Include(s => s.usuarioSeguidor)
                .FirstOrDefaultAsync(m => m.idSeguidor == id);
            if (seguidores == null)
            {
                return NotFound();
            }

            return View(seguidores);
        }

        // POST: Seguidores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.seguidores == null)
            {
                return Problem("Entity set 'Contexto.seguidores'  is null.");
            }
            var seguidores = await _context.seguidores.FindAsync(id);
            if (seguidores != null)
            {
                _context.seguidores.Remove(seguidores);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguidoresExists(int id)
        {
          return (_context.seguidores?.Any(e => e.idSeguidor == id)).GetValueOrDefault();
        }
    }
}
