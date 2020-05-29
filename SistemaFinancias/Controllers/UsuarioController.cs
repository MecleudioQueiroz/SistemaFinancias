using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaFinancias.Models;

namespace SistemaFinancias.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Logar(int? id)
        {
            if (id != null)
            {
                if (id == 0)
                {
                    HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult ValidarLogin(Usuario usuario)
        {
            bool login = usuario.ValidarLogin();

            if (login == true)
            {
                HttpContext.Session.SetString("NomeUsuarioLogado",usuario.Nome);
                HttpContext.Session.SetString("IdUsuarioLogado", usuario.Id.ToString());
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                TempData["MensagemErroLogin"] = "Usuario nao Encontrado";
                return RedirectToAction("Logar");
            }
        }

        [HttpGet]
        public IActionResult NovoUsuario()
        {
            return View();
        }

        [HttpPost]
       public IActionResult NovoUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.NovoUsuario();
                return RedirectToAction("CadastroFinalizado");
            }
            return View();
        }

        public IActionResult CadastroFinalizado()
        {
            return View();
        }
    }
}