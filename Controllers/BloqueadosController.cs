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
    public class BloqueadosController : Controller
    {
        private readonly Contexto _context;

        public BloqueadosController(Contexto context)
        {
            _context = context;
        }

        // GET: Bloqueados
        public async Task<IActionResult> Index()
        {
            var contexto = _context.bloqueados.Include(b => b.bloqueadoUsuario).Include(b => b.bloqueioUsuario);
            return View(await contexto.ToListAsync());
        }

        // GET: Bloqueados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.bloqueados == null)
            {
                return NotFound();
            }

            var bloqueados = await _context.bloqueados
                .Include(b => b.bloqueadoUsuario)
                .Include(b => b.bloqueioUsuario)
                .FirstOrDefaultAsync(m => m.idBloqueio == id);
            if (bloqueados == null)
            {
                return NotFound();
            }

            return View(bloqueados);
        }

        // GET: Bloqueados/Create
        public IActionResult Create()
        {
            ViewData["idUsuarioBloqueado"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            return View();
        }

        // POST: Bloqueados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idBloqueio,idUsuario,idUsuarioBloqueado")] Bloqueados bloqueados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloqueados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUsuarioBloqueado"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuarioBloqueado);
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuario);
            return View(bloqueados);
        }

        // GET: Bloqueados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bloqueados == null)
            {
                return NotFound();
            }

            var bloqueados = await _context.bloqueados.FindAsync(id);
            if (bloqueados == null)
            {
                return NotFound();
            }
            ViewData["idUsuarioBloqueado"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuarioBloqueado);
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuario);
            return View(bloqueados);
        }

        // POST: Bloqueados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idBloqueio,idUsuario,idUsuarioBloqueado")] Bloqueados bloqueados)
        {
            if (id != bloqueados.idBloqueio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloqueados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloqueadosExists(bloqueados.idBloqueio))
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
            ViewData["idUsuarioBloqueado"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuarioBloqueado);
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", bloqueados.idUsuario);
            return View(bloqueados);
        }

        // GET: Bloqueados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bloqueados == null)
            {
                return NotFound();
            }

            var bloqueados = await _context.bloqueados
                .Include(b => b.bloqueadoUsuario)
                .Include(b => b.bloqueioUsuario)
                .FirstOrDefaultAsync(m => m.idBloqueio == id);
            if (bloqueados == null)
            {
                return NotFound();
            }

            return View(bloqueados);
        }

        // POST: Bloqueados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bloqueados == null)
            {
                return Problem("Entity set 'Contexto.bloqueados'  is null.");
            }
            var bloqueados = await _context.bloqueados.FindAsync(id);
            if (bloqueados != null)
            {
                _context.bloqueados.Remove(bloqueados);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloqueadosExists(int id)
        {
          return (_context.bloqueados?.Any(e => e.idBloqueio == id)).GetValueOrDefault();
        }
    }
}
