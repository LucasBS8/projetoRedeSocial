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

        public ActionResult HomePost()
        {
            // Simulating data fetch, replace this with actual data fetch from database
            var posts = new List<Post>
            {
                new Post { postId = 1, postTitulo = "Post 1", postDesc = "Descrição 1", postCor = "#ffcc00", postDate = "2023-06-01", postStatus = "Ativo" },
                new Post { postId = 2, postArquivo = "imagem.jpeg", postCor = "#00ccff", postDate = "2023-06-02", postStatus = "Ativo" },
                // Add more posts
            };

            var postViewModels = new List<Post>();

            foreach (var post in posts)
            {

                postViewModels.Add(new Post
                {

                    postId = post.postId,
                    usuarioPost = post.usuarioPost,
                    postTitulo = post.postTitulo,
                    postDesc = post.postDesc,
                    postArquivo = post.postArquivo,
                    postCor = post.postCor,
                    postDate = post.postDate,
                    postStatus = post.postStatus
                });
            }

            return View(postViewModels);
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
            string? userId = HttpContext.Session.GetString("UserId");
            var post = new Post
            {
                usuarioPost = _context.usuario.Find(int.Parse(userId)),
                usuarioId = int.Parse(userId),
                postDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                postStatus = "Privado" // Valor padrão, pode ser ajustado conforme necessário
            };
            ViewData["usuarioId"] = new SelectList(_context.usuario, "usuarioId", "usuarioId");
            return View(post);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("postId,postTitulo,postDesc,postArquivo,postCor,postDate,postStatus")] Post post)
        {

            if (ModelState.IsValid)
            {
                post.usuarioId = int.Parse(HttpContext.Session.GetString("UserId"));
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
