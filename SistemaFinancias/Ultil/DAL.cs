using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SistemaFinancias.Ultil
{
    public class DAL
    {
        //private static string server = "localhost";
        //private static string database = "junio189_financeiro";
        //private static string user = "junio189";
        //private static string password = "00517549";

        private static string server = "localhost";
        private static string database = "financeiro";
        private static string user = "root";
        private static string password = "00517549";

        private string connectionString = $"Server={server};Database={database};Uid={user};Pwd={password}";

        private MySqlConnection connection;
       
        public DAL()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        //executar select
        public DataTable retDataTable(string sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            return dataTable;
        }

        //Execultar Insert, Delete, Update
        public void ExecultarComandoSql(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
