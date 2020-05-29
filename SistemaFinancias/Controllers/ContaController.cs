using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaFinancias.Models;

namespace SistemaFinancias.Controllers
{
    public class ContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public ContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult ListaConta()
        {
            Conta conta = new Conta(HttpContextAccessor);
            ViewBag.listaConta = conta.ListaConta();
            return View();
        }

        [HttpGet]
        public IActionResult CriarConta(int? id)
        {
            if (id != null)
            {
                Conta conta = new Conta(HttpContextAccessor);
                ViewBag.Registro = conta.BuscarPorId(id);
            }
            return View();
        }

        [HttpPost]
        public IActionResult CriarConta(Conta conta)
        {
            if (ModelState.IsValid)
            {
                conta.HttpContextAccessor = HttpContextAccessor;
                conta.CriarConta();
                return RedirectToAction("ListaConta");
            }
            return View();
        }

        public IActionResult ExcluirConta(int id)
        {
            new Conta().ExcluirConta(id);
            return RedirectToAction("ListaConta");
        }
    }
}