using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
                        Response.Cookies.Append("UserId", user.usuarioId.ToString());
                        HttpContext.Session.SetString("UserId", user.usuarioId.ToString());
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
                // Verifica se o modelo é válido
                if (ModelState.IsValid)
                {
                    // Verifica se o e-mail já está em uso
                    var existingUser = _context.usuario.FirstOrDefault(u => u.usuarioEmail == usuario.usuarioEmail);
                    if (existingUser != null)
                    {
                        // E-mail já está em uso, adiciona mensagem de erro
                        ModelState.AddModelError("usuarioEmail", "Este endereço de e-mail já está sendo usado por outro usuário.");
                        return View("Cadastro", usuario); // Retorna para a view de cadastro com os dados do usuário
                    }

                    // Se o e-mail não está em uso, adiciona o usuário ao contexto e salva as mudanças
                    _context.Add(usuario);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }

                // Se o modelo não for válido, exibe mensagens de erro de validação
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        TempData["Mensagem"] = error.ErrorMessage;
                    }
                }

                return View("Cadastro");
            }
            catch (DbUpdateException dbEx)
            {
                TempData["Mensagem"] = "Erro: " + dbEx.Message;
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
            int valor = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (valor == id)
            {

            if (id == null || _context.usuario == null)
            {
                return NotFound();
            }
            ViewBag.UsuarioId = valor;
            Seguidores? seguidores = _context.seguidores.FirstOrDefault(s => s.idUsuario == id && s.idUsuarioSeguidor == valor);

            ViewBag.Seguidor = seguidores != null;

            ViewData["Posts"] = _context.post.Where(p => p.usuarioId == id).ToList();

            var usuario = await _context.usuario.FirstOrDefaultAsync(m => m.usuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            var seguidoresIds = _context.seguidores.Where(s => s.idUsuarioSeguidor == id).Select(s => s.idUsuario).ToList();
            var seguidoresUsuarios = _context.usuario.Where(u => seguidoresIds.Contains(u.usuarioId)).ToList();
            ViewData["Seguidores"] = seguidoresUsuarios;

            return View(usuario);

            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
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
            int valor = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (valor == id)
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
                            Directory.CreateDirectory(uploadsFolder);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await usuarioImagem.CopyToAsync(fileStream);
                            }
                            usuario.usuarioImagem = "/uploads/" + uniqueFileName;
                        }

                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                        TempData["Mensagem"] = "Erro ao salvar as alterações. Outro usuário pode ter modificado esses dados. Tente novamente.";
                        return View(usuario);
                    }
                    catch (Exception ex)
                    {

                        TempData["Mensagem"] = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                        return View(usuario);
                    }
                    return RedirectToAction("HomePost", "Posts");
                }

                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        TempData["Mensagem"] += error.ErrorMessage + "<br>";
                    }
                }
                return View(usuario);
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
        }



        // GET: Usuario/Delete/5
        [HttpGet]
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
        [HttpPost, ActionName("DeleteConfirmed")]
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
                var postsToDelete = _context.post.Where(p => p.usuarioId == id).ToList();
                var seguidoresToDelete = _context.seguidores.Where(s => s.idUsuario == id).ToList();
                var seguindoToDelete = _context.seguidores.Where(s => s.idUsuarioSeguidor == id).ToList();
                var comentariosToDelete = _context.comentarios.Where(c => c.usuarioId == id).ToList();
                var curtidasToDelete = _context.curtidas.Where(c => c.idUsuario == id).ToList();

                _context.curtidas.RemoveRange(curtidasToDelete);
                _context.comentarios.RemoveRange(comentariosToDelete);
                _context.post.RemoveRange(postsToDelete);
                _context.seguidores.RemoveRange(seguidoresToDelete);
                _context.seguidores.RemoveRange(seguindoToDelete);
                _context.usuario.Remove(usuario);
                _context.SaveChanges();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }


        private bool UsuarioExists(int id)
        {
          return (_context.usuario?.Any(e => e.usuarioId == id)).GetValueOrDefault();
        }
    }
}
