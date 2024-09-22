using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    internal class StudentRepository
    {
        private static StudentRepository instance = null;
        private List<Student> students;

        private StudentRepository()
        {
            students = LoadFromFile();
        }

        public static StudentRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentRepository();
            }
            return instance;
        }

        public List<Student> GetAll()
        {
            return students;
        }

        public Student GetById(int id)
        {
            foreach (Student student in students)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }
            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (Student student in students)
            {
                if (student.Id > maxId)
                {
                    maxId = student.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/StudentFile.csv", false);

                foreach (Student student in students)
                {
                    file.WriteLine(student.StringToCsv());
                }
                file.Flush();
                file.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


        public void Delete(int id)
        {
            Student student = GetById(id);
            if (student == null)
            {
                return;
            }
            students.Remove(student);
            Save();
        }

        public void Update(Student student)
        {
            Student oldStudent = GetById(student.Id);

            if (oldStudent != null)
            {
                oldStudent.Name = student.Name;
                oldStudent.Surname = student.Surname;
                oldStudent.Gender = student.Gender;
                oldStudent.Birthday = student.Birthday;
                oldStudent.Email = student.Email;
                oldStudent.Password = student.Password;
                oldStudent.Profession = student.Profession;
                oldStudent.RegisteredCourses = student.RegisteredCourses;
                oldStudent.RegisteredExams = student.RegisteredExams;
                oldStudent.PenaltyPoints = student.PenaltyPoints;
                oldStudent.AppliedForCourse = student.AppliedForCourse;
                Save();
            }
        }

        public List<Student> Create(Student student)
        {
            student.Id = GenerateId();
            students.Add(student);
            Save();
            //students = LoadFromFile();
            return students;
        }

        public List<Student> LoadFromFile()
        {

            List<Student> students = new List<Student>();

            string filename = "../../../WPF/Data/StudentFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');

                    if (tokens.Length < 11)
                    {
                        continue;
                    }

                    Student student = new Student(
                        id: Int32.Parse(tokens[0]),
                        name: tokens[1],
                        surname: tokens[2],
                        gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), tokens[3]),
                        birthday: DateTime.Parse(tokens[4]),
                        phoneNumber: Int32.Parse(tokens[5]),
                        email: tokens[6],
                        password: tokens[7],
                        profession: (StudentEnum.Profession)Enum.Parse(typeof(StudentEnum.Profession), tokens[8]),
                        registeredCourses: ListToString(tokens[9]),
                        registeredExams: ListToString(tokens[10]),
                        penaltyPoints: Int32.Parse(tokens[11]),
                        appliedForCourse: bool.Parse(tokens[12])
                    );
                    students.Add(student);
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
                int value;
                if (int.TryParse(token, out value))
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
    }
}

