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
    public class CurtidasController : Controller
    {
        private readonly Contexto _context;

        public CurtidasController(Contexto context)
        {
            _context = context;
        }

        // GET: Curtidas
        public async Task<IActionResult> Index()
        {
            var contexto = _context.curtidas.Include(c => c.curtidaUsuario).Include(c => c.postCurtida);
            return View(await contexto.ToListAsync());
        }

        // GET: Curtidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.curtidas == null)
            {
                return NotFound();
            }

            var curtidas = await _context.curtidas
                .Include(c => c.curtidaUsuario)
                .Include(c => c.postCurtida)
                .FirstOrDefaultAsync(m => m.idCurtida == id);
            if (curtidas == null)
            {
                return NotFound();
            }

            return View(curtidas);
        }

        // GET: Curtidas/Create
        public IActionResult Create()
        {
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            ViewData["idPost"] = new SelectList(_context.post, "postId", "postId");
            return View();
        }

        // POST: Curtidas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idCurtida,idUsuario,idPost")] Curtidas curtidas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curtidas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", curtidas.idUsuario);
            ViewData["idPost"] = new SelectList(_context.post, "postId", "postId", curtidas.idPost);
            return View(curtidas);
        }

        // GET: Curtidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.curtidas == null)
            {
                return NotFound();
            }

            var curtidas = await _context.curtidas.FindAsync(id);
            if (curtidas == null)
            {
                return NotFound();
            }
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", curtidas.idUsuario);
            ViewData["idPost"] = new SelectList(_context.post, "postId", "postId", curtidas.idPost);
            return View(curtidas);
        }

        // POST: Curtidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idCurtida,idUsuario,idPost")] Curtidas curtidas)
        {
            if (id != curtidas.idCurtida)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curtidas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurtidasExists(curtidas.idCurtida))
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
            ViewData["idUsuario"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", curtidas.idUsuario);
            ViewData["idPost"] = new SelectList(_context.post, "postId", "postId", curtidas.idPost);
            return View(curtidas);
        }

        // GET: Curtidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.curtidas == null)
            {
                return NotFound();
            }

            var curtidas = await _context.curtidas
                .Include(c => c.curtidaUsuario)
                .Include(c => c.postCurtida)
                .FirstOrDefaultAsync(m => m.idCurtida == id);
            if (curtidas == null)
            {
                return NotFound();
            }

            return View(curtidas);
        }

        // POST: Curtidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.curtidas == null)
            {
                return Problem("Entity set 'Contexto.curtidas'  is null.");
            }
            var curtidas = await _context.curtidas.FindAsync(id);
            if (curtidas != null)
            {
                _context.curtidas.Remove(curtidas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurtidasExists(int id)
        {
          return (_context.curtidas?.Any(e => e.idCurtida == id)).GetValueOrDefault();
        }

    }
}
