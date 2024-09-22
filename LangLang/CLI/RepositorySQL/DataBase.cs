using System;
using Oracle.ManagedDataAccess.Client;

namespace LangLang.RepositorySQL
{
    public class DataBase
    {
        public static string ConnectionString { get; private set; }
        private OracleConnection Connection { get; set; }

        private static DataBase instance = null;

        private DataBase()
        {
            ConnectionString = "\"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.56.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id=jovana;Password=ftn;\"\r\n";

            Connection = new OracleConnection(ConnectionString);
        }

        public static DataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataBase();
                }
                return instance;
            }
        }

        public OracleConnection GetConnection()
        {
            return Connection;
        }

        public bool Connect()
        {
            try
            {
                Connection.Open();
                Console.WriteLine("Successfully connected to Oracle.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error connecting to Oracle: {e.Message}");
                return false;
            }
        }
    }
}
