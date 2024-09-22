using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class StudentService
    {
        private static StudentService instance = null;
        private StudentRepository studentRepository;
        private List<Student> students;
        private StudentService()
        {
            studentRepository = StudentRepository.GetInstance();
            this.students = studentRepository.GetAll();
        }

        public static StudentService GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentService();
            }
            return instance;
        }

        public Student GetById(int id)
        {
            return studentRepository.GetById(id);
        }

        public List<Student> GetAll()
        {
            return studentRepository.GetAll();
        }

        public List<Student> Create(Student student)
        {
            return studentRepository.Create(student);
        }

        public void Update(Student student)
        {
            studentRepository.Update(student);
        }

        public void Delete(int id)
        {
            studentRepository.Delete(id);
        }

        public List<Student> LoadFromFile()
        {

           return studentRepository.LoadFromFile();

        }

        public Student ConvertToStudent(object userObject)
        {
            User user = (User)userObject;

            if (user.Role.ToString() == "Student")
            {
                return GetById(user.RoleId);
            }
            return null;
        }

        public List<Student> GetStudentPenaltyPoints()
        {
            List<Student> studentPenalty = new List<Student>();
            foreach (Student student in students)
            {
                if (student.PenaltyPoints > 0)
                {
                    studentPenalty.Add(student);
                }
            }
            return studentPenalty;
        }
    }
}


