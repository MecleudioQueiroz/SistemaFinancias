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
        public ActionResult NovaTransacao(int? id)
        {
            if (id != null)
            {
                Transacao transacao = new Transacao(HttpContextAccessor);
                ViewBag.Buscar = transacao.BuscarPorId(id);
                
            }

            //Buscando lista de conta e de plano de conta para as views
            ViewBag.ListaConta = new Conta(HttpContextAccessor).ListaConta();
            ViewBag.ListaPlanoConta = new PlanoDeConta(HttpContextAccessor).ListaPlanoContas();
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

        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {
            Transacao transacao = new Transacao(HttpContextAccessor);
            transacao.Excluir(id);
            return RedirectToAction("ListarTransacao");

        }

        [HttpGet]
        [HttpPost]
        public IActionResult Extrato(Transacao transacao)
        {
            transacao.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListarTransacao = transacao.ListaTransacao();
            ViewBag.ListarConta = new Conta(HttpContextAccessor).ListaConta();
            
            return View();
        }

        public IActionResult Dashboard()
        {
            List<Dashboard> Lista = new Dashboard(HttpContextAccessor).Listadashboards();
            string Valores = "";
            string Cores = "";
            string Labels = "";
            var random = new Random();
            for (int i = 0; i < Lista.Count; i++)
            {
                Valores += Lista[i].Total.ToString() + ",";
                Labels += "'" + Lista[i].PlanoConta.ToString() + "',";
                Cores += "'" + String.Format("#{0:X6}", random.Next(0x1000000)) + "',";
            }

            ViewBag.Cores = Cores;
            ViewBag.Labels = Labels;
            ViewBag.Valores = Valores;
            return View();
        }
    }
}