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
        public string Tipo { get; set; }
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

        public List<Transacao> ListaTransacao()
        {
            string UsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            List<Transacao> lista = new List<Transacao>();
            Transacao transacao;
            string sql = $"select t.Id,t.Data,t.Tipo,t.Valor,t.Descricao as historico," +
                         " t.Conta_Id, c.Nome as conta, t.Plano_Contas_Id, p.Descricao as plano_contas" +
                         " from transacao as t inner join conta c " +
                         " on t.Conta_Id = c.Id inner join plano_Contas as p" +
                         " on t.Plano_Contas_Id = p.Id " +
                         $" where t.Usuario_Id = {UsuarioLogado} order by t.Data desc limit 10";
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
                sql = $"update transacao set Data = '{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' Tipo = '{Tipo}' Valor = '{Valor}' Descricao = '{Descricao}' Conta_Id = '{Conta_Id}' Plano_Contas_Id = '{Plano_Contas_Id}' Usuario_Id = {UsuarioLogado} and Id = '{Id}'";
            }
            new DAL().ExecultarComandoSql(sql);
        }
    }
}
