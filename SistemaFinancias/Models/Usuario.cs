using SistemaFinancias.Ultil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SistemaFinancias.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Campo Nome obrigatorio!")]
        public string Nome { get; set; }
        [Required(ErrorMessage ="Campo Email obrigatório!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="O Email informado e invalido!")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Campo Senha obrigatório!")]
        public string Senha { get; set; }
        [Required(ErrorMessage ="Campo Data_Nascimento obrigatório!")]
        public string Data_Nascimento { get; set; }

        public bool ValidarLogin()
        {
            string sql = $"select Id,Nome,Data_Nascimento from usuario where Email = '{Email}' and Senha = '{Senha}'";
            DataTable dt = new DAL().retDataTable(sql);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString());
                    Nome = dt.Rows[0]["Nome"].ToString();
                    Data_Nascimento = dt.Rows[0]["Data_Nascimento"].ToString();
                    return true;
                }
            }

            return false;
        }

        public void NovoUsuario()
        {
            string data_nascimento = DateTime.Parse(Data_Nascimento).ToString("yyyy/MM/dd");
            string sql = $"insert into usuario (Nome, Email, Senha, Data_Nascimento) values ('{Nome}','{Email}','{Senha}','{data_nascimento}')";
            DAL dao = new DAL();
            dao.ExecultarComandoSql(sql);
        }
    }
}
