using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projetoRedeSocial.Models;

namespace projetoRedeSocial.Controllers
{
    public class LoginController : Controller
    {
        private readonly Contexto? _context;
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Entrar(Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View("Index");
            }
            catch (System.Exception ex)
            {
                TempData["Mensagem"] = "Erro: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}