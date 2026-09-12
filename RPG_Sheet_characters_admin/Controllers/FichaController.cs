using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG_Sheet_characters_admin.Models;

namespace RPG_Sheet_characters_admin.Controllers
{
    [Authorize]
    public class FichaController : Controller
    {

        private readonly ScareContext _context;

        public FichaController(ScareContext context)
        {
            _context = context;
        }

        public IActionResult Ficha(int id)
        {
            ViewData["PersonagemId"] = id;
            return View("Ficha");
        }
    }
}
