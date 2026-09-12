using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using RPG_Sheet_characters_admin.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace RPG_Sheet_characters_admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ScareContext _context;
        private readonly PasswordHasher<UsuarioModel> _hasher = new();

        public LoginController(ScareContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login", new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {

            if (!ModelState.IsValid) return View(model);

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuario == model.Usuario);

            var senhaok = user != null && _hasher.VerifyHashedPassword(user, user.Senha, model.Senha) != PasswordVerificationResult.Failed;

            if (!senhaok) {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user!.Usuario),
                new(ClaimTypes.Email, user!.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Listagem", "Agentes", new List<CriarFichaModel>());

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid) return View("Login", new LoginModel());

            var exist = await _context.Usuarios.AnyAsync(u => u.Usuario == model.Usuario || u.Email == model.Email);

            if (exist) { 
                ModelState.AddModelError(string.Empty, "Usuário ou e-mail já cadastrado.");
                ViewBag.ShowRegister = true;
                return View("Login", new LoginModel());
            }

            var senhaHash = _hasher.HashPassword(null!, model.Senha);

            var parametros = new[]
            {
                new MySqlParameter("@p_email", model.Email),
                new MySqlParameter("@p_senha", senhaHash),
                new MySqlParameter("@p_usuario", model.Usuario)
            };

            try
            {

                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_SignUp(@p_email, @p_senha, @p_usuario)", parametros
                );

                TempData["Mensagem"] = "Cadastro realizado! Faça login para continuar.";

                return RedirectToAction("Login");

            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao Alistar Agente {model.Usuario}: {ex.Message}");
                ViewBag.ShowRegister = true;
                Console.WriteLine($"Erro ao Alistar Agente {model.Usuario}: {ex.Message}");
                return View("Login", new LoginModel());
            }


        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", new LoginModel());
        }


    }
}



