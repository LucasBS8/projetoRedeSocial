using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoRedeSocial.Models;

namespace projetoRedeSocial.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Contexto _context;

        public UsuarioController(Contexto context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Entrar(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.usuario
                        .FirstOrDefault(u => u.usuarioEmail == usuario.usuarioEmail && u.usuarioSenha == usuario.usuarioSenha);

                    if (user != null)
                    {
                        HttpContext.Session.SetString("UserId", user.usuarioId.ToString());
                        // Authentication logic here
                        return RedirectToAction("HomePost", "Posts");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Email ou senha inválidos.";
                        return View("Login");
                    }
                }

                return View("Login");
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro: " + ex.Message;
                return View("Login");
            }
        }

        [HttpPost]
        public ActionResult Registrar(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }

                return View("Cadastro");
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro: " + ex.Message;
                return View("Cadastro");
            }
        }


        // GET: Usuario
        public async Task<IActionResult> Index()
        {
              return _context.usuario != null ? 
                          View(await _context.usuario.ToListAsync()) :
                          Problem("Entity set 'Contexto.usuario'  is null.");
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuario
                .FirstOrDefaultAsync(m => m.usuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("usuarioId,usuarioImagem,usuarioDesc,usuarioNome,usuarioTelefone,usuarioEmail,usuarioSenha,usuarioEndereco,usuarioCPF")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("usuarioId,usuarioImagem,usuarioDesc,usuarioNome,usuarioTelefone,usuarioEmail,usuarioSenha,usuarioEndereco,usuarioCPF")] Usuario usuario)
        {
            if (id != usuario.usuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.usuarioId))
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
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuario
                .FirstOrDefaultAsync(m => m.usuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.usuario == null)
            {
                return Problem("Entity set 'Contexto.usuario'  is null.");
            }
            var usuario = await _context.usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.usuario.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.usuario?.Any(e => e.usuarioId == id)).GetValueOrDefault();
        }
    }
}
