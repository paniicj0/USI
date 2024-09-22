using System;
using System.Collections.Generic;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using LangLang.Model;
using LangLang.ModelEnum;

namespace LangLang.WPF.RepositorySQL
{
    public class TutorRepositorySQL
    {
        private readonly string connectionString = DataBase.ConnectionString;
        private List<Tutor> tutors = new List<Tutor>();
        private static TutorRepositorySQL instance = null;

        public TutorRepositorySQL()
        {
            tutors = LoadData();
        }

        public static TutorRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new TutorRepositorySQL();
            }
            return instance;
        }
        public List<Tutor> GetAll()
        {
            // tutors.Clear();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM tutors", conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tutors.Add(new Tutor(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7),
                                languages: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(8)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(9)),
                                tutorCreationDate: reader.GetDateTime(10)
                            ));
                        }
                    }
                }
            }
            return tutors;
        }

        public Tutor GetById(int id)
        {
            Tutor tutor = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM tutors WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tutor = new Tutor(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7),
                                languages: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(8)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(9)),
                                tutorCreationDate: reader.GetDateTime(10)
                            );
                        }
                    }
                }
            }
            return tutor;
        }

        private int GenerateId()
        {
            int maxId = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT MAX(id) FROM tutors", conn))
                {
                    maxId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return maxId + 1;
        }

        public void Save(Tutor tutor)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("INSERT INTO tutors (id, name, surname, gender, birthday, phoneNumber, email, password, languages, languageLevel, tutorCreationDate) VALUES (:id, :name, :surname, :gender, :birthday, :phoneNumber, :email, :password, :languages, :languageLevel, :tutorCreationDate)", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", tutor.Id));
                    cmd.Parameters.Add(new OracleParameter("name", tutor.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", tutor.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", tutor.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", tutor.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", tutor.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", tutor.Email));
                    cmd.Parameters.Add(new OracleParameter("password", tutor.Password));
                    cmd.Parameters.Add(new OracleParameter("languages", tutor.Languages.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", tutor.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("tutorCreationDate", tutor.TutorCreationDate));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("DELETE FROM tutors WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Tutor tutor)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("UPDATE tutors SET name = :name, surname = :surname, gender = :gender, birthday = :birthday, phoneNumber = :phoneNumber, email = :email, password = :password, languages = :languages, languageLevel = :languageLevel, tutorCreationDate = :tutorCreationDate WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", tutor.Id));
                    cmd.Parameters.Add(new OracleParameter("name", tutor.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", tutor.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", tutor.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", tutor.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", tutor.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", tutor.Email));
                    cmd.Parameters.Add(new OracleParameter("password", tutor.Password));
                    cmd.Parameters.Add(new OracleParameter("languages", tutor.Languages.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", tutor.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("tutorCreationDate", tutor.TutorCreationDate));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Tutor Create(Tutor tutor)
        {
            if (GetByEmail(tutor.Email) != null)
            {
                return null;
            }
            tutor.Id = GenerateId();
            Save(tutor);
            return tutor;
        }

        private Tutor GetByEmail(string email)
        {
            Tutor tutor = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM tutors WHERE email = :email", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("email", email));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tutor = new Tutor(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7),
                                languages: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(8)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(9)),
                                tutorCreationDate: reader.GetDateTime(10)
                            );
                        }
                    }
                }
            }
            return tutor;
        }

        public List<Tutor> LoadData()
        {
            List<Tutor> tutors = new List<Tutor>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("SELECT * FROM tutors", conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tutors.Add(new Tutor(
                                    id: reader.GetInt32(0),
                                    name: reader.GetString(1),
                                    surname: reader.GetString(2),
                                    gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                    birthday: reader.GetDateTime(4),
                                    phoneNumber: reader.GetInt32(5),
                                    email: reader.GetString(6),
                                    password: reader.GetString(7),
                                    languages: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(8)),
                                    languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(9)),
                                    tutorCreationDate: reader.GetDateTime(10)
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

            return tutors;
        }

    }

}