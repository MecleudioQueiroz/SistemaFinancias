using Microsoft.AspNetCore.Http;
using SistemaFinancias.Ultil;
using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinancias.Models
{
    public class Transacao
    {
        public int Id { get; set; }       
        public string Data { get; set; }
        public string DataFinal { get; set; } //utilizado para filtros
        public string Tipo { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double Valor { get; set; }

        public string Descricao { get; set; }

        public int Conta_Id { get; set; }
        public string NomeConta { get; set; }

        public int Plano_Contas_Id { get; set; }
        public string DescricaoPlanoConta { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Transacao()
        {

        }

        public Transacao(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Transacao BuscarPorId(int? id)
        {
            Transacao transacao = new Transacao();

            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"Select t.Id,t.Data,t.Tipo,t.Valor,t.Descricao as historico," +
                         " t.Conta_Id, c.Nome as conta, t.Plano_Contas_Id, p.Descricao as plano_contas" +
                         " from transacao as t inner join conta c " +
                         " on t.Conta_Id = c.Id inner join plano_Contas as p" +
                         " on t.Plano_Contas_Id = p.Id " +
                         $" where t.Usuario_Id = {UsuarioLogado} and t.Id='{id}'";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

            transacao = new Transacao();
            transacao.Id = int.Parse(dt.Rows[0]["Id"].ToString());
            transacao.Data = DateTime.Parse(dt.Rows[0]["Data"].ToString()).ToString("yyyy-MM-dd");
            transacao.Tipo = dt.Rows[0]["Tipo"].ToString();
            transacao.Valor = double.Parse(dt.Rows[0]["Valor"].ToString());
            transacao.Descricao = dt.Rows[0]["historico"].ToString();
            transacao.Conta_Id = int.Parse(dt.Rows[0]["Conta_Id"].ToString());
            transacao.NomeConta = dt.Rows[0]["conta"].ToString();
            transacao.Plano_Contas_Id = int.Parse(dt.Rows[0]["Plano_Contas_Id"].ToString());
            transacao.DescricaoPlanoConta = dt.Rows[0]["plano_contas"].ToString();

            return transacao;
        }

        public List<Transacao> ListaTransacao()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            List<Transacao> lista = new List<Transacao>();
            Transacao transacao;

            //utilizado pela view Extrato
            string filtro = "";
            
            if ((Data != null) && (DataFinal != null))
            {
                filtro += $"and t.Data >= '{DateTime.Parse(Data).ToString("yyy/MM/dd")}'and t.Data <= '{DateTime.Parse(DataFinal).ToString("yyy/MM/dd")}'";
            }
            if (Tipo != null)
            {
                if (Tipo != "A")
                {
                    filtro += $"and t.Tipo = '{Tipo}' ";
                }                
            }
            if (Conta_Id != 0)
            {
                filtro += $"and t.Conta_Id = '{Conta_Id}'";
            }

            
             
            //fim

            string sql = $"select t.Id,t.Data,t.Tipo,t.Valor,t.Descricao as historico," +
                         " t.Conta_Id, c.Nome as conta, t.Plano_Contas_Id, p.Descricao as plano_contas" +
                         " from transacao as t inner join conta c " +
                         " on t.Conta_Id = c.Id inner join plano_Contas as p" +
                         " on t.Plano_Contas_Id = p.Id " +
                         $" where t.Usuario_Id = {UsuarioLogado} {filtro} order by t.Data";
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                transacao = new Transacao();
                transacao.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                transacao.Data = DateTime.Parse(dt.Rows[i]["Data"].ToString()).ToString("dd/MM/yyyy");
                transacao.Tipo = dt.Rows[i]["Tipo"].ToString();
                transacao.Valor = double.Parse(dt.Rows[i]["Valor"].ToString());
                transacao.Descricao = dt.Rows[i]["historico"].ToString();
                transacao.Conta_Id = int.Parse(dt.Rows[i]["Conta_Id"].ToString());
                transacao.NomeConta = dt.Rows[i]["conta"].ToString();
                transacao.Plano_Contas_Id = int.Parse(dt.Rows[i]["Plano_Contas_Id"].ToString());
                transacao.DescricaoPlanoConta = dt.Rows[i]["plano_contas"].ToString();

                lista.Add(transacao);
            }
            return lista;

        }

        public void NovaTransacao()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "";
            if (Id == 0)
            {
                sql = $"Insert Into transacao (Data, Tipo, Valor, Descricao, Conta_Id, Plano_Contas_Id, Usuario_Id) values ('{DateTime.Parse(Data).ToString("yyyy/MM/dd")}','{Tipo}','{Valor}','{Descricao}','{Conta_Id}','{Plano_Contas_Id}','{UsuarioLogado}')";               
            }
            else
            {
                sql = $"update transacao set Data = '{DateTime.Parse(Data).ToString("yyyy/MM/dd")}', Tipo = '{Tipo}', Valor = '{Valor}', Descricao = '{Descricao}', Conta_Id = '{Conta_Id}', Plano_Contas_Id = '{Plano_Contas_Id}' where Usuario_Id = {UsuarioLogado} and Id = '{Id}'";
            }
            new DAL().ExecultarComandoSql(sql);
        }

        public void Excluir(int id)
        {
            string sql = $"delete from transacao where Id = '{id}'";
            new DAL().ExecultarComandoSql(sql);
        }
    }

    public class Dashboard
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard()
        {

        }

        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public double Total { get; set; }
        public string PlanoConta { get; set; }

        public List<Dashboard> Listadashboards()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");

            List<Dashboard> Lista = new List<Dashboard>();
            Dashboard item;

            string sql = "select p.Descricao, sum(t.Valor) as Total " + 
            "from transacao as t inner join plano_contas as p " +
            $"on t.Plano_Contas_Id = p.Id where t.Tipo = 'D' and t.Usuario_Id = {UsuarioLogado} " +
            "group by p.Descricao; ";

            DAL dao = new DAL();
            DataTable dt = new DataTable();
            dt = dao.retDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["Total"].ToString());
                item.PlanoConta = dt.Rows[i]["Descricao"].ToString();
                Lista.Add(item);
            }

            return Lista;
        }
    }
}
