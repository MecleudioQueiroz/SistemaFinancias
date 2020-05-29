using Microsoft.AspNetCore.Http;
using SistemaFinancias.Ultil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinancias.Models
{
    public class PlanoDeConta
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int Usuario_Id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public PlanoDeConta()
        {

        }

        public PlanoDeConta(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public PlanoDeConta BuscarPorId (int? id)
        {
            PlanoDeConta planoDeConta = new PlanoDeConta();

            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"Select Id, Descricao, Tipo, Usuario_Id from plano_contas where Usuario_Id = {UsuarioLogado} and Id = {id}";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

           
                planoDeConta.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                planoDeConta.Descricao = dt.Rows[0]["Descricao"].ToString();
                planoDeConta.Tipo = dt.Rows[0]["Tipo"].ToString();
                planoDeConta.Usuario_Id = int.Parse(dt.Rows[0]["Usuario_Id"].ToString());
          
            return planoDeConta;

        }

        public List<PlanoDeConta> ListaPlanoContas()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            List<PlanoDeConta> lista = new List<PlanoDeConta>();
            PlanoDeConta conta;
            string sql = $"Select Id, Descricao, Tipo, Usuario_Id from plano_contas where Usuario_Id = {UsuarioLogado}";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                conta = new PlanoDeConta();
                conta.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                conta.Descricao = dt.Rows[i]["Descricao"].ToString();
                conta.Tipo = dt.Rows[i]["Tipo"].ToString();
                conta.Usuario_Id = int.Parse(dt.Rows[i]["Usuario_Id"].ToString());
                lista.Add(conta);
            }
            return lista;

        }

        public void CriarPlanoConta()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "";
            if (Id == 0)
            {
                sql = $"Insert Into plano_contas(Descricao, Tipo, Usuario_Id) values ('{Descricao}','{Tipo}','{UsuarioLogado}')";
            }
            else
            {
                sql = $"Update  plano_contas set Descricao ='{Descricao}', Tipo = '{Tipo}' where Usuario_Id = '{UsuarioLogado}' and Id = '{Id}'";
            }
            new DAL().ExecultarComandoSql(sql);

        }

        public void ExcluirPlanoConta(int id_plano)
        {
            string sql = $"Delete from plano_contas where id = '{id_plano}'";
            new DAL().ExecultarComandoSql(sql);
        }
    }
}
