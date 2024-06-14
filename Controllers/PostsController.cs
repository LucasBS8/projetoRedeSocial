using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projetoRedeSocial.Models;

namespace projetoRedeSocial.Controllers
{
    public class PostsController : Controller
    {

        private readonly Contexto _context;

        public PostsController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> HomePost()
        {
            ViewData["Usuario"] = _context.usuario.ToList();
            ViewBag.userId = HttpContext.Session.GetString("UserId");
            var contexto = _context.post.Include(p => p.usuarioPost);
            return View(await contexto.ToListAsync());
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var contexto = _context.post.Include(p => p.usuarioPost);
            return View(await contexto.ToListAsync());
        }


        [HttpPost]
        public IActionResult Curtir(int id)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var curtida = _context.curtidas.FirstOrDefault(c => c.idPost == id && c.idUsuario == userId);

            if (curtida == null)
            {
                Curtidas novaCurtida = new()
                {
                    idPost = id,
                    idUsuario = userId
                };
                _context.curtidas.Add(novaCurtida);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public IActionResult Neutro(int id)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var curtida = _context.curtidas.FirstOrDefault(c => c.idPost == id && c.idUsuario == userId);

            if (curtida != null)
            {
                _context.curtidas.Remove(curtida);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }
            ViewBag.userId = HttpContext.Session.GetString("UserId");
            var post = await _context.post
                .Include(p => p.usuarioPost)
                .FirstOrDefaultAsync(m => m.postId == id);
            var comentarios = await _context.comentarios
                .Include(c => c.usuarioComentario)
                .Where(m => m.postId == id)
                .ToListAsync();

            if (post == null)
            {
                return NotFound();
            }

            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            ViewBag.userId = userId;
            ViewBag.Curtida = _context.curtidas.Any(c => c.idPost == id && c.idUsuario == userId);

            // Contagem de curtidas
            ViewBag.CurtidasCount = _context.curtidas.Count(c => c.idPost == id);

            ViewData["Comentarios"] = comentarios;
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarComentario(int postId, string comentario)
        {
            ViewBag.userId = HttpContext.Session.GetString("UserId");
            var post = await _context.post.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            var novoComentario = new Comentarios
            {
                usuarioId = int.Parse(ViewBag.userId),
                postId = postId,
                comentario = comentario,
                data = DateTime.Now.ToString()
            };
            _context.comentarios.Add(novoComentario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = postId });
        }


        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewBag.userId = HttpContext.Session.GetString("UserId");
            var post = new Post
            {
                usuarioPost = _context.usuario.Find(int.Parse(ViewBag.userId)),
                usuarioId = int.Parse(ViewBag.userId),
                postDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                postStatus = "0"
            };
            ViewData["usuarioId"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("postId,postTitulo,postDesc,postCor,postStatus")] Post post, IFormFile? postArquivo)
        {
            post.usuarioId = int.Parse(HttpContext.Session.GetString("UserId"));
            if (ModelState.IsValid)
            {
                if (postArquivo != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + postArquivo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await postArquivo.CopyToAsync(fileStream);
                    }
                    post.postArquivo = "/uploads/" + uniqueFileName;
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomePost));
            }
            return View(post);
        }


        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["usuarioId"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", post.usuarioId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("usuarioId,postTitulo,postDesc,postCor,postDate,postStatus,postArquivo")] Post post, IFormFile? postArquivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await _context.post.AsNoTracking().FirstOrDefaultAsync(p => p.postId == id);

                    if (postArquivo != null)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + postArquivo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        Directory.CreateDirectory(uploadsFolder);

                        
                        if (!string.IsNullOrEmpty(existingPost?.postArquivo))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPost.postArquivo.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await postArquivo.CopyToAsync(fileStream);
                        }
                        post.postArquivo = "/uploads/" + uniqueFileName;
                    }
                    else
                    {
                        post.postArquivo = existingPost?.postArquivo;
                    }

                    post.postId = id;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.postId))
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
            ViewData["usuarioId"] = new SelectList(_context.usuario, "usuarioId", "usuarioId", post.usuarioId);
            return View(post);
        }



        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.post == null)
            {
                return Problem("Entity set 'Contexto.post'  is null.");
            }
            var post = await _context.post.FindAsync(id);
            if (post != null)
            {
                if (!string.IsNullOrEmpty(post.postArquivo))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.postArquivo.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                var comentariosToDelete = _context.comentarios.Where(p => p.postId == id).ToList();
                var curtidasToDelete = _context.curtidas.Where(p => p.idPost == id).ToList();
                _context.comentarios.RemoveRange(comentariosToDelete);
                _context.curtidas.RemoveRange(curtidasToDelete);
                _context.post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(HomePost));
        }

        private bool PostExists(int id)
        {
            return (_context.post?.Any(e => e.postId == id)).GetValueOrDefault();
        }
    }
}
