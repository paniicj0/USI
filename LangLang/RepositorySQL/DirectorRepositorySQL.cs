using LangLang.Model;
using LangLang.ModelEnum;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace LangLang.RepositorySQL
{
    public class DirectorRepositorySQL
    {
        private readonly string connectionString = DataBase.ConnectionString; 
        private static DirectorRepositorySQL instance = null;
        private List<Director> directors = new List<Director>();

        private DirectorRepositorySQL()
        {
            directors = LoadData();
        }

        public static DirectorRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new DirectorRepositorySQL();
            }
            return instance;
        }

        public List<Director> GetAll()
        {
            return directors;
        }

        public Director GetById(int id)
        {
            Director director = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM directors WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            director = new Director(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7)
                            );
                        }
                    }
                }
            }

            return director;
        }

        private int GenerateId()
        {
            int maxId = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT MAX(id) FROM directors";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        maxId = Convert.ToInt32(result);
                    }
                }
            }

            return maxId + 1;
        }

        public void Save(Director director)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO directors (id, name, surname, gender, birthday, phoneNumber, email, password) " +
                             "VALUES (:id, :name, :surname, :gender, :birthday, :phoneNumber, :email, :password)";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", director.Id));
                    cmd.Parameters.Add(new OracleParameter("name", director.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", director.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", director.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", director.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", director.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", director.Email));
                    cmd.Parameters.Add(new OracleParameter("password", director.Password));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM directors WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Director director)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE directors SET name = :name, surname = :surname, gender = :gender, birthday = :birthday, phoneNumber = :phoneNumber, email = :email, password = :password WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", director.Id));
                    cmd.Parameters.Add(new OracleParameter("name", director.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", director.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", director.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", director.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", director.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", director.Email));
                    cmd.Parameters.Add(new OracleParameter("password", director.Password));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Director Create(Director director)
        {
            director.Id = GenerateId();
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO directors (id, name, surname, gender, birthday, phoneNumber, email, password) " +
                             "VALUES (:id, :name, :surname, :gender, :birthday, :phoneNumber, :email, :password)";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", director.Id));
                    cmd.Parameters.Add(new OracleParameter("name", director.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", director.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", director.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", director.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", director.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", director.Email));
                    cmd.Parameters.Add(new OracleParameter("password", director.Password));

                    cmd.ExecuteNonQuery();
                }
            }
            return director;
        }

        public List<Director> LoadData()
        {
            List<Director> directors = new List<Director>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM directors";
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                directors.Add(new Director(
                                    id: reader.GetInt32(0),
                                    name: reader.GetString(1),
                                    surname: reader.GetString(2),
                                    gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                    birthday: reader.GetDateTime(4),
                                    phoneNumber: reader.GetInt32(5),
                                    email: reader.GetString(6),
                                    password: reader.GetString(7)
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Add logging or error handling here
                Console.WriteLine($"Error: {ex.Message}");
            }

            return directors;
        }
    }
}
