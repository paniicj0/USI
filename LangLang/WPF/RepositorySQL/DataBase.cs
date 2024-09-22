using System;
using LangLang.Model;
using System.Windows.Markup;
using Oracle.ManagedDataAccess.Client;

namespace LangLang.WPF.RepositorySQL
{
    public class DataBase
    {
        public static string ConnectionString { get; set; }
        private OracleConnection Connection { get; set; }

        public static DataBase Instance { get; set; } = null;

        public bool Connect()
        {
            try
            {
                string connectionString = "DATA SOURCE=localhost:1521/xepdb1;TNS_ADMIN=C:\\Users\\HP\\Oracle\\network\\admin;USER ID=TEA"; Connection = new OracleConnection(ConnectionString);
                Connection.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public DataBase()
        {
            if (Connect())
            {
                Console.WriteLine("Uspešno konektovanje");
            }
            else
            {
                Console.WriteLine("Neuspešno konektovanje");
            }
        }

        public static DataBase GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataBase();
            }
            return Instance;
        }
    }
}