using SistemaFinancias.Ultil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinancias.Models
{
    public class HomeModels
    {
        public string lerNomeUsuario()
        {
            DAL dal = new DAL();
            DataTable dt = dal.retDataTable("Select * from usuario where Id = 2");

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Nome"].ToString();
                }
            }

            return "Nome nao Encontrado";
        }
    }
}
