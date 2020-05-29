using Microsoft.AspNetCore.Http;
using SistemaFinancias.Ultil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinancias.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Saldo { get; set; }
        public int Usuario_Id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Conta()
        {

        }

        //Recebe o contexto para as variaves de sessão.
        public Conta(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Conta BuscarPorId(int? id)
        {
            Conta conta = new Conta();

            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"select Id, Nome, Saldo, Usuario_Id from conta where Usuario_Id = {UsuarioLogado} and Id = {id}";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

            conta.Id = int.Parse(dt.Rows[0]["Id"].ToString());
            conta.Nome = dt.Rows[0]["Nome"].ToString();
            conta.Saldo = Convert.ToDouble(dt.Rows[0]["Saldo"].ToString());
            conta.Usuario_Id = int.Parse(dt.Rows[0]["Usuario_Id"].ToString());
            return conta;
        }

        public List<Conta> ListaConta()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            List<Conta> lista = new List<Conta>();
            Conta item;
            string sql = $"select Id, Nome, Saldo, Usuario_Id from conta where Usuario_Id = {UsuarioLogado}";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Conta();
                item.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                item.Nome = dt.Rows[i]["Nome"].ToString();
                item.Saldo = double.Parse(dt.Rows[i]["Saldo"].ToString());
                item.Usuario_Id = int.Parse(dt.Rows[i]["Usuario_Id"].ToString());
                lista.Add(item);
            }
            return lista;
        }
        public void CriarConta()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "";
            if (Id == 0)
            {
                sql = $"Insert Into conta (Nome, Saldo, Usuario_Id) values ('{Nome}','{Saldo}','{UsuarioLogado}')";

            }
            else
            {
                sql = $"update conta set Nome = '{Nome}', Saldo = '{Saldo}' where Usuario_Id = '{UsuarioLogado}' and Id = '{Id}'";
            }
            new DAL().ExecultarComandoSql(sql);
        }

        public void ExcluirConta(int id_conta)
        {
            string sql = $"Delete from conta where id = '{id_conta}'";
            new DAL().ExecultarComandoSql(sql);
        }
    }
}
