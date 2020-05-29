using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaFinancias.Models;

namespace SistemaFinancias.Controllers
{
    public class TransacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult ListarTransacao()
        {
            Transacao transacao = new Transacao(HttpContextAccessor);
           ViewBag.ListarTransacao = transacao.ListaTransacao();
            return View();
        }

        [HttpGet]
        public IActionResult NovaTransacao()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NovaTransacao(Transacao transacao)
        {
            if (ModelState.IsValid)
            {
                transacao.HttpContextAccessor = HttpContextAccessor;
                transacao.NovaTransacao();
                return RedirectToAction("ListarTransacao");
            }
            return View();
        }

        public IActionResult Extrato()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}