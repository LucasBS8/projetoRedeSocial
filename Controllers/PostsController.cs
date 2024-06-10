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

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.usuarioPost)
                .FirstOrDefaultAsync(m => m.postId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
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
                Comentarios comentarios = new Comentarios();
                _context.comentarios.Add(comentarios);
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
        public async Task<IActionResult> Edit(int id, [Bind("postId,usuarioId,postTitulo,postDesc,postArquivo,postCor,postDate,postStatus")] Post post)
        {
            if (id != post.postId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.usuarioPost)
                .FirstOrDefaultAsync(m => m.postId == id);
            if (post == null)
            {
                return NotFound();
            }

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
                _context.post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.post?.Any(e => e.postId == id)).GetValueOrDefault();
        }
    }
}
