using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
                        // Definir o cookie (por exemplo, após o login)
                        Response.Cookies.Append("UserId", user.usuarioId.ToString());


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

        [HttpPost]
        public IActionResult Seguir(int id)
        {
            int idUsuarioSeguidor = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (idUsuarioSeguidor != id)
            {
                Seguidores seguidores = new()
                {
                    idUsuario = id,
                    idUsuarioSeguidor = idUsuarioSeguidor,
                };
                _context.seguidores.Add(seguidores);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id });
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeixarDeSeguir(int id)
        {
            int idUsuarioSeguidor = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var seguidor = _context.seguidores
                .FirstOrDefault(s => s.idUsuario == id && s.idUsuarioSeguidor == idUsuarioSeguidor);

            if (seguidor != null)
            {
                _context.seguidores.Remove(seguidor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id });
            }

            return View();
        }



        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }
            int valor = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            ViewBag.UsuarioId = valor;
            Seguidores? seguidores = _context.seguidores.FirstOrDefault(s => s.idUsuario == id && s.idUsuarioSeguidor == valor);

            ViewBag.Seguidor = seguidores != null;

            ViewData["Posts"] = _context.post.Where(p => p.usuarioId == id).ToList();
            var usuario = await _context.usuario.FirstOrDefaultAsync(m => m.usuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        // GET: Usuario/Details/5
        public async Task<IActionResult> DetailsUser(int? id)
        {
            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }
            ViewData["Users"] = _context.usuario.ToList();
            ViewData["Posts"] = _context.post.Where(p => p.usuarioId == id).ToList();
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
        public async Task<IActionResult> Edit(int id, [Bind("usuarioId,usuarioDesc,usuarioNome,usuarioTelefone,usuarioEmail,usuarioSenha,usuarioEndereco,usuarioCPF")] Usuario usuario, IFormFile? usuarioImagem)
        {
            if (id != usuario.usuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (usuarioImagem != null)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + usuarioImagem.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Certifique-se de que o diretório existe
                        Directory.CreateDirectory(uploadsFolder);

                        // Salvar o arquivo no servidor
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await usuarioImagem.CopyToAsync(fileStream);
                        }

                        // Atualizar o caminho da imagem no objeto usuario
                        usuario.usuarioImagem = "/uploads/" + uniqueFileName;
                    }

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
                // Busque todos os posts do usuário
                var postsToDelete = _context.post.Where(p => p.usuarioId == id).ToList();
                var seguidoresToDelete = _context.seguidores.Where(s => s.idUsuario == id).ToList();
                var seguindoToDelete = _context.seguidores.Where(s => s.idUsuarioSeguidor == id).ToList();

                // Exclua os posts
                _context.post.RemoveRange(postsToDelete);
                _context.seguidores.RemoveRange(seguidoresToDelete);
                _context.seguidores.RemoveRange(seguindoToDelete);
                _context.usuario.Remove(usuario);
                _context.SaveChanges();
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
