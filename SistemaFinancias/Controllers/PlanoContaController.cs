using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaFinancias.Models;

namespace SistemaFinancias.Controllers
{
    public class PlanoContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public PlanoContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult ListaPlanoConta()
        {
            PlanoDeConta planoDeConta = new PlanoDeConta(HttpContextAccessor);
            ViewBag.ListaPlanoContas = planoDeConta.ListaPlanoContas();
            return View();
        }

        [HttpGet]
        public ActionResult NovoPlanoDeContas(int? id)
        {
            if (id != null)
            {
                PlanoDeConta planoDeConta = new PlanoDeConta(HttpContextAccessor);
                ViewBag.Registro = planoDeConta.BuscarPorId(id);

            }
            return View();
        }

        [HttpPost]
        public IActionResult NovoPlanoDeContas(PlanoDeConta planoDeConta)
        {
            if (ModelState.IsValid)
            {
                planoDeConta.HttpContextAccessor = HttpContextAccessor;
                planoDeConta.CriarPlanoConta();
                return RedirectToAction("ListaPlanoConta");
            }
            return View();
        }

        public IActionResult ExcluirPlanoConta(int id)
        {
            PlanoDeConta plano = new PlanoDeConta(HttpContextAccessor);
            plano.ExcluirPlanoConta(id);
            return RedirectToAction("ListaPlanoConta");
        }
    }
}