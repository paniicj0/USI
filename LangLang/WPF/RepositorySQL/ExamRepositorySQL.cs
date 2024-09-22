using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using LangLang.Model;
using LangLang.ModelEnum;
using System.IO;
using LangLang.WPF.RepositorySQL;

namespace LangLang.WPF.RepositorySQL
{
    public class ExamRepositorySQL
    {
        private readonly string connectionString = DataBase.ConnectionString;
        private List<Exam> exams = new List<Exam>();
        private static ExamRepositorySQL instance = null;

        public ExamRepositorySQL()
        {
            exams = LoadData();
        }

        public static ExamRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new ExamRepositorySQL();
            }
            return instance;
        }
        public List<Exam> LoadData()
        {
            // exams.Clear(); // Clear existing list before loading
            List<Exam> exams = new List<Exam>();

            try
            {

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM exams";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                exams.Add(new Exam(
                                    id: reader.GetInt32(0),
                                    language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(1)),
                                    languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(2)),
                                    numOfStudents: reader.GetInt32(3),
                                    examDate: reader.GetDateTime(4),
                                    examTime: reader.GetTimeSpan(5),
                                    examDuration: reader.GetInt32(6),
                                    tutorId: reader.GetInt32(7),
                                    numberOfAppliedStudents: reader.GetInt32(8),
                                    confirmed: reader.GetBoolean(9)
                                ));
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");  
            }

            return exams;
        }
        public List<Exam> GetAll()
        {
            List<Exam> exams = new List<Exam>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM exams";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam(
                                id: reader.GetInt32(0),
                                language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(1)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(2)),
                                numOfStudents: reader.GetInt32(3),
                                examDate: reader.GetDateTime(4),
                                examTime: reader.GetTimeSpan(5),
                                examDuration: reader.GetInt32(6),
                                tutorId: reader.GetInt32(7),
                                numberOfAppliedStudents: reader.GetInt32(8),
                                confirmed: reader.GetBoolean(9)
                            ));
                        }
                    }
                }
            }

            return exams;
        }

        public Exam GetById(int id)
        {
            Exam exam = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM exams WHERE id = :id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            exam = new Exam(
                                id: reader.GetInt32(0),
                                language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(1)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(2)),
                                numOfStudents: reader.GetInt32(3),
                                examDate: reader.GetDateTime(4),
                                examTime: reader.GetTimeSpan(5),
                                examDuration: reader.GetInt32(6),
                                tutorId: reader.GetInt32(7),
                                numberOfAppliedStudents: reader.GetInt32(8),
                                confirmed: reader.GetBoolean(9)
                            );
                        }
                    }
                }
            }

            return exam;
        }

        public void Save(Exam exam)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO exams (id, language, languageLevel, numOfStudents, examDate, examTime, examDuration, tutorId, numberOfAppliedStudents, confirmed) " +
                             "VALUES (:id, :language, :languageLevel, :numOfStudents, :examDate, :examTime, :examDuration, :tutorId, :numberOfAppliedStudents, :confirmed)";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", exam.Id));
                    cmd.Parameters.Add(new OracleParameter("language", exam.Language.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", exam.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("numOfStudents", exam.NumOfStudents));
                    cmd.Parameters.Add(new OracleParameter("examDate", exam.ExamDate));
                    cmd.Parameters.Add(new OracleParameter("examTime", exam.ExamTime));
                    cmd.Parameters.Add(new OracleParameter("examDuration", exam.ExamDuration));
                    cmd.Parameters.Add(new OracleParameter("tutorId", exam.TutorId));
                    cmd.Parameters.Add(new OracleParameter("numberOfAppliedStudents", exam.NumberOfAppliedStudents));
                    cmd.Parameters.Add(new OracleParameter("confirmed", exam.Confirmed));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Exam exam)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE exams SET language = :language, languageLevel = :languageLevel, numOfStudents = :numOfStudents, " +
                             "examDate = :examDate, examTime = :examTime, examDuration = :examDuration, tutorId = :tutorId, " +
                             "numberOfAppliedStudents = :numberOfAppliedStudents, confirmed = :confirmed WHERE id = :id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", exam.Id));
                    cmd.Parameters.Add(new OracleParameter("language", exam.Language.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", exam.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("numOfStudents", exam.NumOfStudents));
                    cmd.Parameters.Add(new OracleParameter("examDate", exam.ExamDate));
                    cmd.Parameters.Add(new OracleParameter("examTime", exam.ExamTime));
                    cmd.Parameters.Add(new OracleParameter("examDuration", exam.ExamDuration));
                    cmd.Parameters.Add(new OracleParameter("tutorId", exam.TutorId));
                    cmd.Parameters.Add(new OracleParameter("numberOfAppliedStudents", exam.NumberOfAppliedStudents));
                    cmd.Parameters.Add(new OracleParameter("confirmed", exam.Confirmed));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM exams WHERE id = :id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}