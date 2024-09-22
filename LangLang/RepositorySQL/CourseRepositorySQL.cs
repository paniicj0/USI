using LangLang.Model;
using LangLang.ModelEnum;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LangLang.RepositorySQL
{
    public class CourseRepositorySQL
    {
        private readonly string connectionString = DataBase.ConnectionString; 
        private static CourseRepositorySQL instance = null;
        private List<Course> courses = new List<Course>();

        public event EventHandler CourseAdded;

        private CourseRepositorySQL()
        {
            courses = LoadData();
        }

        public static CourseRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new CourseRepositorySQL();
            }
            return instance;
        }

        public List<Course> GetAll()
        {
            return courses;
        }

        public Course GetById(int id)
        {
            Course course = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM courses WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            course = new Course(
                                id: reader.GetInt32(0),
                                language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(1)),
                                languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(2)),
                                duration: reader.GetInt32(3),
                                days: ParseDaysEnum(reader.GetString(4)),
                                start: reader.GetDateTime(5),
                                realization: (RealizationEnum.Realization)Enum.Parse(typeof(RealizationEnum.Realization), reader.GetString(6)),
                                maxStudents: reader.GetInt32(7),
                                tutorId: reader.GetInt32(8),
                                numberOfStudents: reader.GetInt32(9)
                            );
                        }
                    }
                }
            }

            return course;
        }

        public void Save(Course course)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO courses (id, language, languageLevel, duration, days, start_date, realization, max_students, tutor_id, number_of_students) 
                       VALUES (:id, :language, :languageLevel, :duration, :days, :startDate, :realization, :maxStudents, :tutorId, :numberOfStudents)";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", GenerateId())); // Automatski generiši novi ID
                    cmd.Parameters.Add(new OracleParameter("language", course.Language.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", course.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("duration", course.Duration));
                    cmd.Parameters.Add(new OracleParameter("days", string.Join(",", course.Days.Select(d => d.ToString()))));
                    cmd.Parameters.Add(new OracleParameter("startDate", course.Start));
                    cmd.Parameters.Add(new OracleParameter("realization", course.Realization.ToString()));
                    cmd.Parameters.Add(new OracleParameter("maxStudents", course.MaxStudents));
                    cmd.Parameters.Add(new OracleParameter("tutorId", course.TutorId));
                    cmd.Parameters.Add(new OracleParameter("numberOfStudents", course.NumberOfStudents));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Delete(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM courses WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Course course)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE courses SET language = :language, languageLevel = :languageLevel, duration = :duration, days = :days, start = :start, realization = :realization, maxStudents = :maxStudents, tutorId = :tutorId, numberOfStudents = :numberOfStudents WHERE id = :id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", course.Id));
                    cmd.Parameters.Add(new OracleParameter("language", course.Language.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", course.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("duration", course.Duration));
                    cmd.Parameters.Add(new OracleParameter("days", FormatDaysEnum(course.Days)));
                    cmd.Parameters.Add(new OracleParameter("start", course.Start));
                    cmd.Parameters.Add(new OracleParameter("realization", course.Realization.ToString()));
                    cmd.Parameters.Add(new OracleParameter("maxStudents", course.MaxStudents));
                    cmd.Parameters.Add(new OracleParameter("tutorId", course.TutorId));
                    cmd.Parameters.Add(new OracleParameter("numberOfStudents", course.NumberOfStudents));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Course Create(Course course)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO courses (id, language, languageLevel, duration, days, start, realization, maxStudents, tutorId, numberOfStudents) " +
                             "VALUES (:id, :language, :languageLevel, :duration, :days, :start, :realization, :maxStudents, :tutorId, :numberOfStudents)";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    course.Id = GenerateId();
                    cmd.Parameters.Add(new OracleParameter("id", course.Id));
                    cmd.Parameters.Add(new OracleParameter("language", course.Language.ToString()));
                    cmd.Parameters.Add(new OracleParameter("languageLevel", course.LanguageLevel.ToString()));
                    cmd.Parameters.Add(new OracleParameter("duration", course.Duration));
                    cmd.Parameters.Add(new OracleParameter("days", FormatDaysEnum(course.Days)));
                    cmd.Parameters.Add(new OracleParameter("start", course.Start));
                    cmd.Parameters.Add(new OracleParameter("realization", course.Realization.ToString()));
                    cmd.Parameters.Add(new OracleParameter("maxStudents", course.MaxStudents));
                    cmd.Parameters.Add(new OracleParameter("tutorId", course.TutorId));
                    cmd.Parameters.Add(new OracleParameter("numberOfStudents", course.NumberOfStudents));

                    cmd.ExecuteNonQuery();
                }
            }
            CourseAdded?.Invoke(this, EventArgs.Empty);
            return course;
        }

        private int GenerateId()
        {
            int maxId = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT MAX(id) FROM courses";
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

        private List<DaysEnum.Days> ParseDaysEnum(string daysString)
        {
            List<DaysEnum.Days> days = new List<DaysEnum.Days>();
            string[] dayTokens = daysString.Split(',');
            foreach (string token in dayTokens)
            {
                days.Add((DaysEnum.Days)Enum.Parse(typeof(DaysEnum.Days), token.Trim()));
            }
            return days;
        }

        private string FormatDaysEnum(List<DaysEnum.Days> days)
        {
            return string.Join(",", days.Select(d => d.ToString()));
        }

        public List<Course> LoadData()
        {
            List<Course> courses = new List<Course>();

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM courses";
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courses.Add(new Course(
                                    id: reader.GetInt32(0),
                                    language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), reader.GetString(1)),
                                    languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), reader.GetString(2)),
                                    duration: reader.GetInt32(3),
                                    days: ParseDaysEnum(reader.GetString(4)),
                                    start: reader.GetDateTime(5),
                                    realization: (RealizationEnum.Realization)Enum.Parse(typeof(RealizationEnum.Realization), reader.GetString(6)),
                                    maxStudents: reader.GetInt32(7),
                                    tutorId: reader.GetInt32(8),
                                    numberOfStudents: reader.GetInt32(9)
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

            return courses;
        }
    }
}
