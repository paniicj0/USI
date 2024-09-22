using LangLang.Model;
using LangLang.ModelEnum;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace LangLang.RepositorySQL
{
    public class StudentRepositorySQL
    {
        private static StudentRepositorySQL instance = null;
        private List<Student> students;
        private readonly string connectionString = DataBase.ConnectionString;

        private StudentRepositorySQL()
        {
            students = LoadData();
        }

        public static StudentRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentRepositorySQL();
            }
            return instance;
        }

        public List<Student> GetAll()
        {
            return students;
        }

        public Student GetById(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM students WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7),
                                profession: (StudentEnum.Profession)Enum.Parse(typeof(StudentEnum.Profession), reader.GetString(8)),
                                registeredCourses: ListToString(reader.GetString(9)),
                                registeredExams: ListToString(reader.GetString(10)),
                                penaltyPoints: reader.GetInt32(11),
                                appliedForCourse: reader.GetBoolean(12)
                            );
                        }
                    }
                }
            }
            return null;
        }

        private int GenerateId()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT MAX(id) FROM students", conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt32(result) + 1;
                    }
                }
            }
            return 1;
        }

        public void Save(Student student)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(
                    "INSERT INTO students (id, name, surname, gender, birthday, phoneNumber, email, password, profession, registeredCourses, registeredExams, penaltyPoints, appliedForCourse) " +
                    "VALUES (:id, :name, :surname, :gender, :birthday, :phoneNumber, :email, :password, :profession, :registeredCourses, :registeredExams, :penaltyPoints, :appliedForCourse)", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", student.Id));
                    cmd.Parameters.Add(new OracleParameter("name", student.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", student.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", student.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", student.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", student.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", student.Email));
                    cmd.Parameters.Add(new OracleParameter("password", student.Password));
                    cmd.Parameters.Add(new OracleParameter("profession", student.Profession.ToString()));
                    cmd.Parameters.Add(new OracleParameter("registeredCourses", StringifyList(student.RegisteredCourses)));
                    cmd.Parameters.Add(new OracleParameter("registeredExams", StringifyList(student.RegisteredExams)));
                    cmd.Parameters.Add(new OracleParameter("penaltyPoints", student.PenaltyPoints));
                    cmd.Parameters.Add(new OracleParameter("appliedForCourse", student.AppliedForCourse));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("DELETE FROM students WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Student student)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(
                    "UPDATE students SET name = :name, surname = :surname, gender = :gender, birthday = :birthday, phoneNumber = :phoneNumber, email = :email, password = :password, " +
                    "profession = :profession, registeredCourses = :registeredCourses, registeredExams = :registeredExams, penaltyPoints = :penaltyPoints, appliedForCourse = :appliedForCourse " +
                    "WHERE id = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", student.Id));
                    cmd.Parameters.Add(new OracleParameter("name", student.Name));
                    cmd.Parameters.Add(new OracleParameter("surname", student.Surname));
                    cmd.Parameters.Add(new OracleParameter("gender", student.Gender.ToString()));
                    cmd.Parameters.Add(new OracleParameter("birthday", student.Birthday));
                    cmd.Parameters.Add(new OracleParameter("phoneNumber", student.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("email", student.Email));
                    cmd.Parameters.Add(new OracleParameter("password", student.Password));
                    cmd.Parameters.Add(new OracleParameter("profession", student.Profession.ToString()));
                    cmd.Parameters.Add(new OracleParameter("registeredCourses", StringifyList(student.RegisteredCourses)));
                    cmd.Parameters.Add(new OracleParameter("registeredExams", StringifyList(student.RegisteredExams)));
                    cmd.Parameters.Add(new OracleParameter("penaltyPoints", student.PenaltyPoints));
                    cmd.Parameters.Add(new OracleParameter("appliedForCourse", student.AppliedForCourse));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> Create(Student student)
        {
            student.Id = GenerateId();
            Save(student);
            students = LoadData();
            return students;
        }

        public List<Student> LoadData()
        {
            List<Student> students = new List<Student>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT * FROM students", conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student(
                                id: reader.GetInt32(0),
                                name: reader.GetString(1),
                                surname: reader.GetString(2),
                                gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), reader.GetString(3)),
                                birthday: reader.GetDateTime(4),
                                phoneNumber: reader.GetInt32(5),
                                email: reader.GetString(6),
                                password: reader.GetString(7),
                                profession: (StudentEnum.Profession)Enum.Parse(typeof(StudentEnum.Profession), reader.GetString(8)),
                                registeredCourses: ListToString(reader.GetString(9)),
                                registeredExams: ListToString(reader.GetString(10)),
                                penaltyPoints: reader.GetInt32(11),
                                appliedForCourse: reader.GetBoolean(12)
                            ));
                        }
                    }
                }
            }
            return students;
        }

        private List<int> ListToString(string data)
        {
            List<int> dataList = new List<int>();
            string[] tokens = data.Split(',');

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int value))
                {
                    dataList.Add(value);
                }
                else
                {
                    Console.WriteLine($"Failed to parse: {token}");
                }
            }
            return dataList;
        }

        private string StringifyList(List<int> data)
        {
            return string.Join(",", data);
        }
    }
}
